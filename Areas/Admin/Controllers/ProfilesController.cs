using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.Repository;
using TracyShop.ViewModels;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfilesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly ILoginRepository _loginRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ProfilesController(ILoginRepository loginRepository, UserManager<AppUser> userManager, AppDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _loginRepository = loginRepository;
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Profiles()
        {
            var userid = _userManager.GetUserId(HttpContext.User);

            if (userid == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                AppUser user = _userManager.FindByIdAsync(userid).Result;
                ProfileModel profile = new ProfileModel();
                profile.Name = user.Name;
                profile.UserName = user.UserName;
                profile.Email = user.Email;
                profile.Birthday = user.Birthday;
                profile.PhoneNumber = user.PhoneNumber;
                profile.Gender = user.Gender;
                profile.AvatarPath = null;
                return View(profile);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Profiles(ProfileModel userdetails)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            AppUser user = _userManager.FindByIdAsync(userid).Result;

            string wwwRootPath = _hostingEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(userdetails.AvatarPath.FileName);
            string extension = Path.GetExtension(userdetails.AvatarPath.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string filePath = Path.Combine(wwwRootPath + "/Admin/img/avatar/", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await userdetails.AvatarPath.CopyToAsync(fileStream);
            }

            user.Name = userdetails.Name;
            user.PhoneNumber = userdetails.PhoneNumber;
            user.Gender = userdetails.Gender;
            user.Birthday = userdetails.Birthday;
            user.Avatar = "/Admin/img/avatar/" + fileName;

            userdetails.Email = user.Email;
            userdetails.UserName = user.UserName;
            IdentityResult x = await _userManager.UpdateAsync(user);
            if (x.Succeeded)
            {
                ViewBag.Message = $"Update user information successed.";
                return View("profiles", userdetails);
            }
            else
            {
                ViewBag.Message = $"Update user information failed.";
                return View(userdetails);
            }
        }
    }
}
