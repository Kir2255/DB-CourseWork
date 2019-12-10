using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LK5.Models
{
    public partial class Income
    {
        public Income()
        {
            FamilyMembers = new HashSet<FamilyMember>();
        }
        [Display(Name = "ID")]
        public int IncomeId { get; set; }
        [Display(Name = "Income Name")]
        public int? IncomeSourceId { get; set; }
        [Required]
        [Range(0, 99999999999)]
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? IncomeDate { get; set; }

        public virtual IncomeSource IncomeSource { get; set; }
        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
    }
}
