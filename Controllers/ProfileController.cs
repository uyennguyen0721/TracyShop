using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public ProfileController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [Authorize]
        [Route("profile", Name = "profile")]
        public ActionResult Profile()
        {
            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
