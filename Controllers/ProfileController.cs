using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;
using TracyShop.Repository;
using TracyShop.ViewModels;

namespace TracyShop.Controllers
{
    public class ProfileController : Controller
    {

        private readonly ILoginRepository _loginRepository;
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(ILoginRepository loginRepository, UserManager<AppUser> userManager)
        {
            _loginRepository = loginRepository;
            _userManager = userManager;
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
                return View(user);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("profile", Name = "profile")]
        public async Task<IActionResult> Profile(AppUser userdetails)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            AppUser user = _userManager.FindByIdAsync(userid).Result;
            user.Name = userdetails.Name;
            user.PhoneNumber = userdetails.PhoneNumber;
            user.Gender = userdetails.Gender;
            user.Birthday = userdetails.Birthday;
            IdentityResult x = await _userManager.UpdateAsync(user);
            if (x.Succeeded)
            {
                ViewBag.Message = $"Update user information successed";
                return View(user);
            }
            else
            {
                ViewBag.Message = $"Update user information failed";
                return View(user);
            }
        }


        [Authorize]
        [Route("profile/address", Name = "address")]
        public ActionResult Address()
        {
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

        [Authorize]
        [Route("profile/reset-password", Name = "reset-password")]
        // GET: ProfileController/ResetPassword
        public ActionResult ResetPassword()
        {
            return View();
        }
    }
}
