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
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");
            ViewBag.name = HttpContext.Session.GetString("name");

            var modelContext = _context.Testimonials.Include(t => t.Status).Include(t => t.Users);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Status)
                .Include(t => t.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            ViewData["UsersId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,UsersId,StatusId")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                testimonial.UsersId = Convert.ToDecimal(HttpContext.Session.GetInt32("userid"));
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction("Testimonials", "Home");
            }
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", testimonial.StatusId);
            ViewData["UsersId"] = new SelectList(_context.Users, "Id", "Id", testimonial.UsersId);
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Status1", testimonial.StatusId);
            ViewData["UsersId"] = new SelectList(_context.Users, "Id", "Fname", testimonial.UsersId);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Message,UsersId,StatusId")] Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
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
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", testimonial.StatusId);
            ViewData["UsersId"] = new SelectList(_context.Users, "Id", "Id", testimonial.UsersId);
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Status)
                .Include(t => t.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
            return _context.Testimonials.Any(e => e.Id == id);
        }
    }
}
