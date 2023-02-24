using IronPdf;
using MailKit.Net.Smtp;
using MailKit.Search;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using MimeKit;
using store.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace store.Controllers
{
    public class HomeController : Controller
    {

        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _logger = logger;
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        
        public IActionResult Index()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");

            var test = _context.Testimonials.Where(x => x.StatusId == 1).ToList();
            var product = _context.Products.Where(x => x.StatusId1 == 1).ToList();
            var user = _context.Users.ToList();
            var category = _context.Categories.ToList();
            var models = Tuple.Create<IEnumerable<Category>,IEnumerable<Product>,IEnumerable<Testimonial>>(category,product,test);
            ViewBag.Name = HttpContext.Session.GetString("name");
            ViewBag.UserId = HttpContext.Session.GetInt32("userid");
            ViewBag.UserId2 = HttpContext.Session.GetInt32("adminid");
            var id2 = _context.Users.Where(x => x.Id == HttpContext.Session.GetInt32("userid")).FirstOrDefault();
            if (id2 != null)
            {
                ViewBag.UserID2 = id2.RoleId;
            }
            else
            {
                ViewBag.UserID2 = null;
            }
            var id = _context.Users.Where(x => x.Id == HttpContext.Session.GetInt32("adminid")).FirstOrDefault();
            if (id != null)
            {
                ViewBag.UserID = id.RoleId;
            }
            else
            {
                ViewBag.UserID = null;
            }
            
            return View(models);
        }

    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //product Get
        public IActionResult product (int id)
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            var result = _context.Products.Where(x => x.CategoryId == id && x.StatusId1 == 1).ToList();

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
        public async Task<IActionResult> product (string find, int id)
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");

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
            var model = _context.Products.Where(x => x.Title.Contains(find) && x.CategoryId == id && x.StatusId1 == 1).ToList();
            return View(model);
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
            var id2 = _context.Users.Where(x => x.Id == HttpContext.Session.GetInt32("userid")).FirstOrDefault();
            if (id2 != null)
            {
                ViewBag.UserID2 = id2.RoleId;
            }
            else
            {
                ViewBag.UserID2 = null;
            }
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            var model = _context.Products.Where(x => x.Title.Contains(find) && x.StatusId1 == 1).ToList();
            return View(model);
        }

        //about
        public async Task<IActionResult> about()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = Convert.ToDecimal(HttpContext.Session.GetInt32("userid"));

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
            return View(await _context.Abouts.ToListAsync());
        }

        //Testimonials
        public IActionResult Testimonials()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
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
            return View();
        }
       
        //Contact Us
        public IActionResult contacts()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = Convert.ToDecimal(HttpContext.Session.GetInt32("userid"));
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
            return View();
        }


        //All Invoice for Customer 
        public IActionResult invoice()
        {           
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            var userid = HttpContext.Session.GetInt32("userid");
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

            var CustomerInvoice = _context.Orders.Where(x => x.UserId == userid).ToList();
            return View(CustomerInvoice);
        }



        //GET: Orders/Create
        public IActionResult orders()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");           
            var order = _context.Orders.Include(x=>x.Product).ToList();

            return View(order);
        }
        // POST: Orders

        public IActionResult add(decimal id)
        {

            var userId = Convert.ToDecimal(HttpContext.Session.GetInt32("userid"));

            Order order = new Order();
            order.ProductId = id;
            order.UserId = userId;
            order.Datefrom = DateTime.Today;
            order.Dateto = DateTime.Now;
            order.Quantity = 1;
            order.PayId = 1;
            order.Status = 1;
            _context.Add(order);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(ourproduct));

        } 

        //Emails
        public IActionResult Checkout()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            var user = HttpContext.Session.GetInt32("userid");
            var test = _context.Orders.Where(x => x.UserId == user && x.Status == 1).Include(x=>x.User).ToList();
            var email = HttpContext.Session.GetString("userEmail");
            var order = _context.Orders.Include(x => x.Product).ToList();
            var balance = 3000;
            decimal total = 0;
            var theOrder = " ";
            var date = DateTime.Now;
            var fname =" ";
            var lname = " ";
            foreach (var item in test)
            {
                var customer = _context.Users.Where(x=>x.Id ==  item.UserId).FirstOrDefault();
                fname = customer.Fname;
                lname = customer.Lname;
                var UserEmail = customer.Email;
                total += Convert.ToDecimal(item.Quantity * item.Product.Price);
                theOrder += item.Product.Title + "-";
                date = item.Datefrom.Value;
            };
            if (total < balance)
            {                
                foreach(var item in test) {
                    
                    item.Status = 0;
                    _context.Update(item);
                    _context.SaveChangesAsync();
                }
                var Renderer = new ChromePdfRenderer();
                var pdf = Renderer.RenderHtmlAsPdf($" <h1> Thank you for your purchase from our store. </h1> \n\r <h1> The Total Price for your purchase is : $ {total} " +
                    $" </h1> \n\r <h1> <p> Number of Product is :{test.Count()} </p> Product names : {theOrder}  </h1> <p>Order date From{date} </p>" +
                    $"<p> Customer name is: {fname} {lname} </p>" +
                    $"<p> Welcome To Awesome Magazine Store. </p>" );
                pdf.SaveAs("Invoice.pdf");

                string x = "Thank you for purchasing from our website. We hope you like our service";

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Store", "webmvc.2@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Invoice";
                var builder = new BodyBuilder();
                builder.TextBody = x;
                builder.HtmlBody = "<p> Thank you for purchasing from our website. We hope you like our service </p>";
                builder.Attachments.Add(@"C:\Users\User\Desktop\ABOOD\store\store\Invoice.pdf");

                message.Body = builder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                try
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("webmvc.2@gmail.com", "ksthywyzlbvibhbk");
                    client.Send(message);

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }

            }
            return RedirectToAction(nameof(Index));
        }
        
        
        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Profile()
        {
            var cusid = HttpContext.Session.GetInt32("userid");
            var profile = _context.Users.Where(x => x.Id == cusid).FirstOrDefault();
            return View(profile);
        }
       
        //Edit
        [HttpPost]
        public IActionResult Profile(String Fname, String Lname, IFormFile ImageFile, String email, String Password ,int rolid)
        {
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            var cusid = HttpContext.Session.GetInt32("userid");
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

    }
}

