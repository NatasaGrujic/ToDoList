using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Data;

namespace ToDoList.Pages
{
    [Authorize] // Om ej inloggad, tvingar användaren att gå till inloggningssidan
    public class IndexModel : PageModel
    {
        // Instansvariabler (databaskontexten i konstruktorn sparas i våra variabler nedan)
        private ApplicationDbContext _ctx;
        private readonly UserManager<IdentityUser> _um;

        public IndexModel(ApplicationDbContext ctx, UserManager<IdentityUser> um) // Konstruktor databaskontexten och user manager är endast tillgängliga här
        {
            _ctx = ctx; // Hämtar databaskontexten så att vi kan använda den (dependency injection)
            _um = um; // Hämtar användarens namn
        }

        public IEnumerable<ToDoList.Models.Task> Tasks { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        public string UserName { get; set; }
        [BindProperty]
        public ToDoList.Models.Task NewTask { get; set; }
        public async Task OnGetAsync()
        {
            var user = await _um.GetUserAsync(User); // Vi anropar _um vår egen instans
            UserName = await _um.GetUserNameAsync(user); // Hämtar användarnamnet
            var userId = await _um.GetUserIdAsync(user); // = User id
            Tasks = await _ctx.Tasks.Where(u => u.UserId == userId).ToListAsync();
            UserId = userId;
        }
        public async Task<IActionResult> OnGetRemoveAsync(int id)
        {
            var taskToDelete = await _ctx.Tasks.FindAsync(id); 
            
            // Felhantering 
            if(taskToDelete == null) return RedirectToPage("/Index");

            var user = await _um.GetUserAsync(User); // Vi anropar _um vår egen instans
            var userId = await _um.GetUserIdAsync(user); // = User id
            if(taskToDelete.UserId != userId) return RedirectToPage("/Index");

            _ctx.Tasks.Remove(taskToDelete);
            await _ctx.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _ctx.Tasks.AddAsync(NewTask);
            await _ctx.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}
