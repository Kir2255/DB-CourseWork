using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LK5.Models
{
    public partial class FamilyMember
    {
        [Display(Name = "ID")]
        public int MemberId { get; set; }
        [Required]
        [Display(Name = "FIO")]
        public string Fio { get; set; }
        [RegularExpression(@"^м|ж")]
        [StringLength(1)]
        [Display(Name = "Sex")]
        public string Sex { get; set; }
        [Range(1, 200)]
        [Display(Name = "Age")]
        public int? Age { get; set; }
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Income Name")]
        public int? IncomeId { get; set; }
        [Display(Name = "Expense Name")]
        public int? ExpenseId { get; set; }
        [Required]
        [Range(0, 9999999999999)]
        [Display(Name = "Balance")]
        public decimal? Balance { get; set; }

        public virtual Expense Expense { get; set; }
        public virtual Income Income { get; set; }
    }
}
