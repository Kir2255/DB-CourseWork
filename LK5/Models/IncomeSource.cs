using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LK5.Models
{
    public partial class IncomeSource
    {
        public IncomeSource()
        {
            Incomes = new HashSet<Income>();
        }
        [Display(Name = "ID")]
        public int IncomeSourceId { get; set; }
        [Required]
        [Display(Name = "Income Name")]
        public string IncomeName { get; set; }
        [Required]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        public virtual ICollection<Income> Incomes { get; set; }
    }
}
