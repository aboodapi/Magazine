using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using store.Models;

namespace store.Controllers
{
    public class AboutsController : Controller
    {
        private readonly ModelContext _context;

        public AboutsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Abouts
        public async Task<IActionResult> Index()
        {
            var id = _context.Users.Where(x => x.Id == HttpContext.Session.GetInt32("adminid")).FirstOrDefault();
            if (id != null)
            {
                ViewBag.UserID = id.RoleId;
            }
            else
            {
                ViewBag.UserID = null;

            }

            var id2 = _context.Users.Where(x => x.Id == HttpContext.Session.GetInt32("userid")).FirstOrDefault();
            if (id2 != null)
            {
                ViewBag.UserID2 = id2.RoleId;
            }
            else
            {
                ViewBag.UserID2 = null;

            }
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            return View(await _context.Abouts.ToListAsync());
        }

        // GET: Abouts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // GET: Abouts/Create
        public IActionResult Create()
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            return View();
        }

        // POST: Abouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,About1")] About about)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (ModelState.IsValid)
            {
                _context.Add(about);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }

        // GET: Abouts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts.FindAsync(id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        // POST: Abouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,About1")] About about)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (id != about.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(about.Id))
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
            return View(about);
        }

        // GET: Abouts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // POST: Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            var about = await _context.Abouts.FindAsync(id);
            _context.Abouts.Remove(about);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutExists(decimal id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            return _context.Abouts.Any(e => e.Id == id);
        }
    }
}
