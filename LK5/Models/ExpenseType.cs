using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LK5.Models
{
    public partial class ExpenseType
    {
        public ExpenseType()
        {
            Expenses = new HashSet<Expense>();
        }
        [Display(Name = "ID")]
        public int ExpenseTypeId { get; set; }
        [Required]
        [Display(Name = "Expense Name")]
        public string ExpenseName { get; set; }
        [Required]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
