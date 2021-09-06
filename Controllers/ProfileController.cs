using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.Repository;
using TracyShop.ViewModels;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace TracyShop.Controllers
{
    public class ProfileController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly ILoginRepository _loginRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ProfileController(ILoginRepository loginRepository, UserManager<AppUser> userManager, AppDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _loginRepository = loginRepository;
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Authorize]
        [Route("profile", Name = "profile")]
        public IActionResult Profile()
        {
            var userid = _userManager.GetUserId(HttpContext.User);

            if(userid == null)
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
        [Authorize]
        [Route("profile", Name = "profile")]
        public async Task<IActionResult> Profile(ProfileModel userdetails)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            AppUser user = _userManager.FindByIdAsync(userid).Result;

            string wwwRootPath = _hostingEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(userdetails.AvatarPath.FileName);
            string extension = Path.GetExtension(userdetails.AvatarPath.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string filePath = Path.Combine(wwwRootPath + "/img/avatar/", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await userdetails.AvatarPath.CopyToAsync(fileStream);
            }

            user.Name = userdetails.Name;
            user.PhoneNumber = userdetails.PhoneNumber;
            user.Gender = userdetails.Gender;
            user.Birthday = userdetails.Birthday;
            user.Avatar = "/img/avatar/" + fileName;

            userdetails.Email = user.Email;
            userdetails.UserName = user.UserName;
            IdentityResult x = await _userManager.UpdateAsync(user);
            if (x.Succeeded)
            {
                ViewBag.Message = $"Update user information successed.";
                return View("profile", userdetails);
            }
            else
            {
                ViewBag.Message = $"Update user information failed.";
                return View(userdetails);
            }
        }



        [Authorize]
        [HttpGet]
        [Route("profile/change-address", Name = "change-address")]
        public IActionResult ChangeAddress()
        {
            //var userid = _userManager.GetUserId(HttpContext.User);

            //if (userid == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            //else
            //{
            //    ChangeAddressModel changeAddress = new ChangeAddressModel();
            //    var address = _context.Address.Where(a => a.User.Id == userid).FirstOrDefault();
            //    changeAddress.Id = address.Id;
            //    changeAddress.SpecificAddress = address.SpecificAddress;
            //    changeAddress.SelectDistrict = address.District;
            //    changeAddress.SelectCity = address.City;

            //    return View();
            //}

            return View();


        }


        [Authorize]
        [Route("profile/change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost("profile/change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginRepository.ChangePasswordAsync(model);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }
    }
}
