using store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using NuGet.Frameworks;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace magazine.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;

        private readonly ModelContext _context;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }
        public IActionResult ourproduct()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            var result = _context.Products.Where(x => x.StatusId1 == 1).ToList();

            var id1 = _context.Users.Where(x => x.Id == HttpContext.Session.GetInt32("adminid")).FirstOrDefault();
            if (id1 != null)
            {
                ViewBag.UserID = id1.RoleId;
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
            return View(result);
        }
        [HttpPost]
        public IActionResult ourproduct(string find, int id)
        {
            var model = _context.Products.Where(x => x.Title.Contains(find) && x.StatusId1 == 1).ToList();
            return View(model);
        }


        public IActionResult contacthome()
        {
            return View();
        }

        //contact : admin
        public IActionResult contact()
        {
            var data = _context.Contacts.ToList();
            return View(data);
        }

        public IActionResult info()
        {
            ViewBag.adminUser = HttpContext.Session.GetString("AdminUsername");
            ViewBag.countUser = _context.Users.Count();
            ViewBag.contact = _context.Contacts.Count();
            ViewBag.test = _context.Testimonials.Count();
            ViewBag.magazinescount = _context.Products.Count();
            ViewBag.publish = _context.Products.Where(x => x.StatusId1 == 1).Count();
            ViewBag.unpublish = _context.Products.Where(x => x.StatusId1 == 2).Count();
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");
            ViewBag.orders = _context.Orders.Count();
            return View();
        }

        public IActionResult Home()
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");
            var test = _context.Testimonials.Where(x => x.StatusId == 1).ToList();
            var product = _context.Products.Where(x => x.StatusId1 == 1).ToList();
            var user = _context.Users.ToList();
            var category = _context.Categories.ToList();
            var models = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>, IEnumerable<Testimonial>>(category, product, test);
            return View(models);
        }

        public IActionResult Profile()
        {
            var cusid = HttpContext.Session.GetInt32("adminid");
            var profile = _context.Users.Where(x => x.Id == cusid).FirstOrDefault();
            return View(profile);
        }

        //Edit
        [HttpPost]
        public IActionResult Profile(String Fname, String Lname, IFormFile ImageFile, String email, String Password, int rolid)
        {
            ViewBag.userid = HttpContext.Session.GetInt32("adminid");
            var cusid = HttpContext.Session.GetInt32("adminid");
            if (ModelState.IsValid)
            {
                var profile = _context.Users.Where(x => x.Id == cusid).FirstOrDefault();

                if (ImageFile != null)
                {
                    string wwwrootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    { ImageFile.CopyToAsync(filestream); }
                    profile.Imagepath = fileName;
                }
                if (profile.Fname != Fname)
                {
                    profile.Fname = Fname;
                }
                if (profile.Lname != Lname)
                {
                    profile.Lname = Lname;
                }
                if (profile.Email != email)
                {
                    profile.Email = email;
                }
                if (profile.Password != Password)
                {
                    profile.Password = Password;
                }
                _context.Update(profile);
                _context.SaveChangesAsync();
                return View(profile);
            }
            return RedirectToAction("invoice", "Home");
        }

        private bool UserExists(decimal id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        //about
        public async Task<IActionResult> Aboutus()
        {
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            return View(await _context.Abouts.ToListAsync());

        }

        [HttpGet]
        public IActionResult Report()
            {  
            
            ViewBag.adminid = HttpContext.Session.GetInt32("adminid");

            var modelcontext = _context.Orders.Include(p => p.User).Include(c => c.Product).ToList();

                var user = _context.Users.ToList();
                var product = _context.Products.ToList();
                var order = _context.Orders.ToList();
                var category = _context.Categories.ToList();
                var card = _context.Payments.ToList();
                var data = from u in user
                           join p in product on u.Id equals p.Id
                           join c in category on p.CategoryId equals c.Id
                           join o in order on p.Id equals o.ProductId
                           join pay in card on o.PayId equals pay.Id
                           select new invoice { product = p, order = o, user = u, category = c, payment = pay };
                var models = Tuple.Create<IEnumerable<invoice>, IEnumerable<Order>>(data, modelcontext);

            return View(models);
            }

        [HttpPost]
        public async Task<IActionResult> report(DateTime? stratDate, DateTime? endDate)
            { 
                var user = _context.Users.ToList();
                var product = _context.Products.ToList();
                var order = _context.Orders.ToList();
                var category = _context.Categories.ToList();
                var card = _context.Payments.ToList();
                var data = from u in user
                           join p in product on u.Id equals p.Id
                           join c in category on p.CategoryId equals c.Id
                           join o in order on p.Id equals o.ProductId
                           join pay in card on o.PayId equals pay.Id
                           select new invoice { product = p, order = o, user = u, category = c, payment = pay };

                var modelContext = _context.Orders.Include(p => p.User).Include(p => p.Product);
               
                if (stratDate == null && endDate == null)
                {

                    ViewBag.totalquantity = modelContext.Sum(x => x.Quantity);
                    ViewBag.totalprice = modelContext.Sum(x => x.Product.Price * x.Quantity);
                 
                    var models = Tuple.Create<IEnumerable<invoice>, IEnumerable<Order>>(data,await modelContext.ToListAsync());
                        return View(models);
                }
                else if (stratDate == null && endDate != null)
                {
                    ViewBag.totalquantity = modelContext.Where(x=>x.Datefrom.Value.Date== stratDate).Sum(x => x.Quantity);                 
                    ViewBag.totalprice = modelContext.Where(x=>x.Datefrom.Value.Date== stratDate).Sum(x =>x.Product.Price * x.Quantity);
                 
                    var models = Tuple.Create<IEnumerable<invoice>, IEnumerable<Order>>(data, await modelContext.Where(x => x.Datefrom.Value.Date == stratDate).ToListAsync());
                        return View(models);
                }
                else if (stratDate != null && endDate == null)
                {
                    ViewBag.totalquantity = modelContext.Where(x => x.Datefrom.Value.Date == stratDate).Sum(x => x.Quantity);
                    ViewBag.totalprice = modelContext.Where(x => x.Datefrom.Value.Date == stratDate).Sum(x => x.Product.Price * x.Quantity);

                    var models = Tuple.Create<IEnumerable<invoice>, IEnumerable<Order>>(data, await modelContext.Where(x => x.Datefrom.Value.Date == stratDate).ToListAsync());
                    return View(models);
                }
                else
                {
                    ViewBag.totalquantity = modelContext.Where(x => x.Datefrom.Value.Date >= stratDate && x.Datefrom.Value.Date <= endDate).Sum(x => x.Quantity);
                    ViewBag.totalprice = modelContext.Where(x => x.Datefrom.Value.Date >= stratDate && x.Datefrom.Value.Date <= endDate).Sum(x => x.Product.Price * x.Quantity);

                    var models = Tuple.Create<IEnumerable<invoice>, IEnumerable<Order>>(data, await modelContext.Where(x => x.Datefrom.Value.Date >= stratDate && x.Datefrom.Value.Date <= endDate).ToListAsync());
                    return View(models);
                }
            }
                


    }
}
