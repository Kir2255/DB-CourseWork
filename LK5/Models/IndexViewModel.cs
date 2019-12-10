using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LK5.Models
{
    public class IndexViewModel
    {
        public List<IncomeSource> IncomeSources { get; set; }
        public List<Income> Incomes { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<ExpenseType> ExpenseTypes { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
