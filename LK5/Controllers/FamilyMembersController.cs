using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LK5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LK5.Controllers
{
    public class FamilyMembersController : Controller
    {
        private readonly HomeBookkeepingContext context;
        public FamilyMembersController(HomeBookkeepingContext _context)
        {
            context = _context;
        }

        // GET: FamilyMembers
        [ResponseCache(Duration = 252)]
        public async Task<IActionResult> Index(string fio, string sex, string age, string phone, string expense, string income, int? balance,  int page = 1)
        {
            int pageSize = 20;

            var sources = context.FamilyMembers.Include(c => c.Expense.ExpenseType).Include(r => r.Income.IncomeSource).ToList();
            var count = sources.Count();

            List<FamilyMember> items = null;
            if (!String.IsNullOrEmpty(fio) || !String.IsNullOrEmpty(sex) || !String.IsNullOrEmpty(phone) || !String.IsNullOrEmpty(age)
                || !String.IsNullOrEmpty(expense) || !String.IsNullOrEmpty(income))
            {
                if (!String.IsNullOrEmpty(fio))
                {
                    items = sources.Where(r => r.Fio.Contains(fio)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!String.IsNullOrEmpty(sex))
                {
                    items = sources.Where(r => r.Sex.Contains(sex)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!String.IsNullOrEmpty(phone))
                {
                    items = sources.Where(r => r.Phone.Contains(phone)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!String.IsNullOrEmpty(age))
                {
                    items = sources.Where(r => r.Age.ToString().Equals(age)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!String.IsNullOrEmpty(expense))
                {
                    items = sources.Where(r => r.Expense.ExpenseType.ExpenseName.Contains(age)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!String.IsNullOrEmpty(income))
                {
                    items = sources.Where(r => r.Income.IncomeSource.IncomeName.Contains(age)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            else
            {
                items = sources.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                FamilyMembers = items
            };

            return View(viewModel);
        }

        public async Task<IActionResult> IncomeSources()
        {
            return View(await context.FamilyMembers.ToListAsync());
        }

        // GET: FamilyMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var source = await context.FamilyMembers.Include(r => r.Income.IncomeSource).Include(r => r.Expense.ExpenseType)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (source == null)
            {
                return NotFound();
            }

            return View(source);
        }

        // GET: FamilyMembers/Create
        public IActionResult Create()
        {
            ViewData["ExpenseId"] = new SelectList(context.Expenses.Include(c => c.ExpenseType), "ExpenseId", "ExpenseType.ExpenseName");
            ViewData["IncomeId"] = new SelectList(context.Incomes.Include(c => c.IncomeSource), "IncomeId", "IncomeSource.IncomeName");
            return View();
        }

        // POST: FamilyMembers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Fio,Sex,Age,Phone,IncomeId,ExpenseId,Balance")] FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {
                context.Add(familyMember);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExpenseId"] = new SelectList(context.Expenses.Include(c => c.ExpenseType), "ExpenseId", "ExpenseType.ExpenseName", familyMember.ExpenseId);
            ViewData["IncomeId"] = new SelectList(context.Incomes.Include(c => c.IncomeSource), "IncomeId", "IncomeSource.IncomeName", familyMember.IncomeId);
            return View();
        }

        // GET: FamilyMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyMember = await context.FamilyMembers.FindAsync(id);
            if (familyMember == null)
            {
                return NotFound();
            }

            ViewData["ExpenseId"] = new SelectList(context.Expenses.Include(c => c.ExpenseType), "ExpenseId", "ExpenseType.ExpenseName", familyMember.ExpenseId);
            ViewData["IncomeId"] = new SelectList(context.Incomes.Include(c => c.IncomeSource), "IncomeId", "IncomeSource.IncomeName", familyMember.IncomeId);
            return View(familyMember);
        }

        // POST: FamilyMembers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Fio,Sex,Age,Phone,IncomeId,ExpenseId,Balance")] FamilyMember familyMember)
        {
            if (id != familyMember.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(familyMember);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamilyMemberExists(familyMember.MemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExpenseId"] = new SelectList(context.Expenses.Include(c => c.ExpenseType), "ExpenseId", "ExpenseType.ExpenseName", familyMember.ExpenseId);
            ViewData["IncomeId"] = new SelectList(context.Incomes.Include(c => c.IncomeSource), "IncomeId", "IncomeSource.IncomeName", familyMember.IncomeId);
            return View(familyMember);
        }

        // GET: FamilyMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyMember = await context.FamilyMembers.Include(c => c.Income).Include(c => c.Expense)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (familyMember == null)
            {
                return NotFound();
            }

            return View(familyMember);
        }

        // POST: FamilyMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var familyMember = await context.FamilyMembers.FindAsync(id);
            context.FamilyMembers.Remove(familyMember);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FamilyMemberExists(int id)
        {
            return context.FamilyMembers.Any(e => e.MemberId == id);
        }
    }
}