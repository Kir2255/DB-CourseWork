using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LK5.Models
{
    public partial class Expense
    {
        public Expense()
        {
            FamilyMembers = new HashSet<FamilyMember>();
        }
        [Display(Name = "ID")]
        [Required]
        public int ExpenseId { get; set; }
        [Required]
        [Display(Name = "Expense Type")]
        public int? ExpenseTypeId { get; set; }
        [Required]
        [Range(0, 99999999999)]
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? ExpenseDate { get; set; }

        public virtual ExpenseType ExpenseType { get; set; }
        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
    }
}
