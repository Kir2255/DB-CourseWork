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
    public class IncomeSourcesController : Controller
    {
        private readonly HomeBookkeepingContext context;

        public IncomeSourcesController(HomeBookkeepingContext _context)
        {
            context = _context;
        }

        // GET: IncomeSources
        [ResponseCache(Duration = 252)]
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int pageSize = 20;

            var sources = context.IncomeSources.ToList();
            var count = sources.Count();
            List<IncomeSource> items;
            if (!String.IsNullOrEmpty(name))
            {
                items = sources.Where(r => r.IncomeName.Contains(name)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                items = sources.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                IncomeSources = items
            };

            return View(viewModel);
        }

        public async Task<IActionResult> IncomeSources()
        {
            return View(await context.IncomeSources.ToListAsync());
        }

        // GET: IncomeSources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var source = await context.IncomeSources
                .FirstOrDefaultAsync(m => m.IncomeSourceId == id);
            if (source == null)
            {
                return NotFound();
            }

            return View(source);
        }

        // GET: IncomeSources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IncomeSources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IncomeSourceId,IncomeName,Comment")] IncomeSource source)
        {
            if (ModelState.IsValid)
            {
                context.Add(source);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(source);
        }

        // GET: IncomeSources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await context.IncomeSources.FindAsync(id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // POST: IncomeSources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncomeSourceId,IncomeName,Comment")] IncomeSource source)
        {
            if (id != source.IncomeSourceId)
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
                    if (!IncomeSourceExists(source.IncomeSourceId))
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

        // GET: IncomeSources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await context.IncomeSources
                .FirstOrDefaultAsync(m => m.IncomeSourceId == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // POST: IncomeSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var call = await context.IncomeSources.FindAsync(id);
            context.IncomeSources.Remove(call);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomeSourceExists(int id)
        {
            return context.IncomeSources.Any(e => e.IncomeSourceId == id);
        }
    }
}