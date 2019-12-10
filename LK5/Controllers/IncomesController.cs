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
    public class IncomesController : Controller
    {
        private readonly HomeBookkeepingContext context;
        public IncomesController(HomeBookkeepingContext _context)
        {
            context = _context;
        }

        // GET: Incomes
        [ResponseCache(Duration = 252)]
        public async Task<IActionResult> Index(string name, int? amount, int page = 1)
        {
            int pageSize = 20;

            var sources = context.Incomes.Include(c => c.IncomeSource).ToList();
            var count = sources.Count();

            List<Income> items = null;
            if (!String.IsNullOrEmpty(name) || amount.HasValue)
            {
                if (amount.HasValue)
                {
                    items = sources.OrderBy(r => r.Amount).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!String.IsNullOrEmpty(name))
                {
                    items = sources.Where(r => r.IncomeSource.IncomeName.Contains(name)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
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
                Incomes = items
            };

            return View(viewModel);
        }

        public async Task<IActionResult> IncomeSources()
        {
            return View(await context.Incomes.ToListAsync());
        }

        // GET: Incomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var source = await context.Incomes.Include(r => r.IncomeSource)
                .FirstOrDefaultAsync(m => m.IncomeId == id);
            if (source == null)
            {
                return NotFound();
            }

            return View(source);
        }

        // GET: Incomes/Create
        public IActionResult Create()
        {
            ViewData["IncomeSourceId"] = new SelectList(context.IncomeSources, "IncomeSourceId", "IncomeName");
            return View();
        }

        // POST: Incomes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IncomeId,IncomeSourceId,Amount,IncomeDate")] Income income)
        {
            if (ModelState.IsValid)
            {
                context.Add(income);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IncomeSourceId"] = new SelectList(context.IncomeSources, "IncomeSourceId", "IncomeName", income.IncomeSourceId);
            return View();
        }

        // GET: Incomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            ViewData["IncomeSourceId"] = new SelectList(context.IncomeSources, "IncomeSourceId", "IncomeName", income.IncomeSourceId);
            return View(income);
        }

        // POST: Incomes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncomeId,IncomeSourceId,Amount,IncomeDate")] Income income)
        {
            if (id != income.IncomeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(income);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeExists(income.IncomeId))
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

            ViewData["IncomeSourceId"] = new SelectList(context.IncomeSources, "IncomeSourceId", "IncomeName", income.IncomeSourceId);
            return View(income);
        }

        // GET: Incomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await context.Incomes.Include(c => c.IncomeSource)
                .FirstOrDefaultAsync(m => m.IncomeId == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // POST: Incomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var income = await context.Incomes.FindAsync(id);
            context.Incomes.Remove(income);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomeExists(int id)
        {
            return context.Incomes.Any(e => e.IncomeId == id);
        }
    }
}