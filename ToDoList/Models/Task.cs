using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Task
    {
        public int ID { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        public bool isDone { get; set; }
        [Required]
        [StringLength(240)]
        public string UserId { get; set; }
    }
}
