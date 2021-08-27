using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;
using TracyShop.Services;

namespace TracyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public HomeController(
            IUserService userService,
            IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
