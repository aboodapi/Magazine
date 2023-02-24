using store.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using Chilkat;

namespace magazine.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;

        private readonly ModelContext _context;

        public LoginRegisterController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Fname,Lname,Email,Password,Imagepath,RoleId,ImageFile")] User user)
        {
            var email = _context.Users.Where(x=>x.Email == user.Email).FirstOrDefault();
            ViewBag.msg = string.Format("This Email is ALready Used !!");
            if (email == null)
            {
                if (ModelState.IsValid)
                {
                    if (user.ImageFile != null)
                    {
                        string wwwrootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await user.ImageFile.CopyToAsync(filestream);
                        }
                        user.Imagepath = fileName;
                    }
                    user.RoleId = 2;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "LoginRegister");
                }
            }
            return View(user);
        }

        //Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] User user)
        {
            ViewBag.msg = string.Format("Your Email Or Password is Incorrect !!");
            var auth = _context.Users.Where(x => x.Password == user.Password && x.Email == user.Email).SingleOrDefault();
            if (auth != null)
            {
                switch (auth.RoleId)
                {
                    case 1: //Admin
                        HttpContext.Session.SetString("AdminUsername", auth.Email);
                        HttpContext.Session.SetInt32("adminid", Convert.ToInt32(auth.Id));
                        return RedirectToAction("Home", "Admin");
                    case 2://Customer
                        HttpContext.Session.SetString("userEmail",auth.Email);
                        HttpContext.Session.SetString("name", auth.Fname);
                        HttpContext.Session.SetInt32("userid", Convert.ToInt32(auth.Id));
                        return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View();
            }
            return View();
        }
    }
}
