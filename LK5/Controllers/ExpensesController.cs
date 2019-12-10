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
    public class ExpensesController : Controller
    {
        private readonly HomeBookkeepingContext context;
        public ExpensesController(HomeBookkeepingContext _context)
        {
            context = _context;
        }

        // GET: Expenses
        [ResponseCache(Duration = 252)]
        public async Task<IActionResult> Index(string name, int? amount, int page = 1)
        {
            int pageSize = 20;

            var sources = context.Expenses.Include(c => c.ExpenseType).ToList();
            var count = sources.Count();

            List<Expense> items = null;
            if (!String.IsNullOrEmpty(name) || amount.HasValue)
            {
                if (amount.HasValue)
                {
                    items = sources.OrderBy(r => r.Amount).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!String.IsNullOrEmpty(name))
                {
                    items = sources.Where(r => r.ExpenseType.ExpenseName.Contains(name)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
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
                Expenses = items
            };

            return View(viewModel);
        }

        public async Task<IActionResult> IncomeSources()
        {
            return View(await context.Expenses.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var source = await context.Expenses.Include(r => r.ExpenseType)
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (source == null)
            {
                return NotFound();
            }

            return View(source);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            ViewData["ExpenseTypeId"] = new SelectList(context.ExpenseTypes, "ExpenseTypeId", "ExpenseName");
            return View();
        }

        // POST: Expenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseId,ExpenseTypeId,Amount,ExpenseDate")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                context.Add(expense);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExpenseTypeId"] = new SelectList(context.ExpenseTypes, "ExpenseTypeId", "ExpenseName", expense.ExpenseTypeId);
            return View();
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await context.Expenses.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            ViewData["ExpenseTypeId"] = new SelectList(context.ExpenseTypes, "ExpenseTypeId", "ExpenseName", income.ExpenseTypeId);
            return View(income);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseId, ExpenseTypeId, Amount, ExpenseDate")] Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(expense);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
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

            ViewData["ExpenseTypeId"] = new SelectList(context.ExpenseTypes, "ExpenseTypeId", "ExpenseName", expense.ExpenseTypeId);
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await context.Expenses.Include(c => c.ExpenseType)
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var income = await context.Expenses.FindAsync(id);
            context.Expenses.Remove(income);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return context.Expenses.Any(e => e.ExpenseId == id);
        }
    }
}