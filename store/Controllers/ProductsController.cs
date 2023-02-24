using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using store.Models;

namespace store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;

        private readonly ModelContext _context;

        public ProductsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            var modelContext = _context.Products.Include(p => p.Category).Include(p => p.StatusId1Navigation);
            return View(await modelContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(string find, int id)
        {
            ViewBag.userid = HttpContext.Session.GetInt32("adminid");            
            var model = _context.Products.Where(x => x.Title.Contains(find)).ToList();
            return View(model);
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.StatusId1Navigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["StatusId1"] = new SelectList(_context.Statuses, "Id", "Status1");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PublicationDate,Price,Imagepath,CategoryId,StatusId1,ImageFile")] Product product)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(filestream);
                    }
                    product.Imagepath = fileName;

                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            ViewData["StatusId1"] = new SelectList(_context.Statuses, "Id", "Id", product.StatusId1);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["StatusId1"] = new SelectList(_context.Statuses, "Id", "Status1", product.StatusId1);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Title,PublicationDate,Price,Imagepath,CategoryId,StatusId1,ImageFile")] Product product)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ImageFile != null)
                    {
                        string wwwrootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(filestream);
                        }
                        product.Imagepath = fileName;

                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["StatusId1"] = new SelectList(_context.Statuses, "Id", "Status1", product.StatusId1);
            return View(product);
        }


        ////Edit
        //[HttpPost]
        //public IActionResult Edit(String Title, DateTime PublicationDate, IFormFile ImageFile, int Price, int CategoryId, int StatusId1)
        //{
        //    ViewBag.userid = HttpContext.Session.GetInt32("adminid");

        //    if (ModelState.IsValid)
        //    {
        //        var product = _context.Products.FirstOrDefault();

        //        if (ImageFile != null)
        //        {
        //            string wwwrootPath = _webHostEnviroment.WebRootPath;
        //            string fileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
        //            string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
        //            using (var filestream = new FileStream(path, FileMode.Create))
        //            { ImageFile.CopyToAsync(filestream); }
        //            product.Imagepath = fileName;
        //        }
        //        if (product.Title != Title)
        //        {
        //            product.Title = Title;
        //        }
        //        if (product.PublicationDate != PublicationDate)
        //        {
        //            product.PublicationDate = PublicationDate;
        //        }
        //        if (product.Price != Price)
        //        {
        //            product.Price = Price;
        //        }
        //        if (product.CategoryId != CategoryId)
        //        {
        //            product.CategoryId = CategoryId;
        //        }
        //        _context.Update(product);
        //        _context.SaveChangesAsync();
        //        return View(product);
        //    }
        //    return RedirectToAction("Index", "products");
        //}


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.StatusId1Navigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(decimal id)
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            return _context.Products.Any(e => e.Id == id);
        }
    }
}
