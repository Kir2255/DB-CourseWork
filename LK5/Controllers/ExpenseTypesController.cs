using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LK5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LK5.Controllers
{
    public class ExpenseTypesController : Controller
    {
        private readonly HomeBookkeepingContext context;
        public ExpenseTypesController(HomeBookkeepingContext _context)
        {
            context = _context;
        }

        // GET: ExpenseTypes
        [ResponseCache(Duration = 252)]
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int pageSize = 20;

            var sources = context.ExpenseTypes.ToList();
            var count = sources.Count();

            List<ExpenseType> items;
            if (!String.IsNullOrEmpty(name))
            {
                items = sources.Where(r => r.ExpenseName.Contains(name)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                items = sources.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                ExpenseTypes = items
            };

            return View(viewModel);
        }

        public async Task<IActionResult> IncomeSources()
        {
            return View(await context.ExpenseTypes.ToListAsync());
        }

        // GET: ExpenseTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var source = await context.ExpenseTypes
                .FirstOrDefaultAsync(m => m.ExpenseTypeId == id);
            if (source == null)
            {
                return NotFound();
            }

            return View(source);
        }

        // GET: ExpenseTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpenseTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseTypeId,ExpenseName,Comment")] ExpenseType source)
        {
            if (ModelState.IsValid)
            {
                context.Add(source);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(source);
        }

        // GET: ExpenseTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await context.ExpenseTypes.FindAsync(id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // POST: ExpenseTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseTypeId,ExpenseName,Comment")] ExpenseType source)
        {
            if (id != source.ExpenseTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(source);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseTypeExists(source.ExpenseTypeId))
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

            return View(source);
        }

        // GET: ExpenseTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await context.ExpenseTypes
                .FirstOrDefaultAsync(m => m.ExpenseTypeId == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // POST: ExpenseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var call = await context.ExpenseTypes.FindAsync(id);
            context.ExpenseTypes.Remove(call);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseTypeExists(int id)
        {
            return context.ExpenseTypes.Any(e => e.ExpenseTypeId == id);
        }
    }
}