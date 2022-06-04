using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ILogger<ReviewsController> _logger;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private IHostingEnvironment _hostingEnvironment;

        public ReviewsController(ILogger<ReviewsController> logger, AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: ReviewsController/Create
        [HttpGet]
        [Authorize]
        [Route("reviews", Name = "reviews")]
        public IActionResult Create(int id)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            AppUser user = _userManager.FindByIdAsync(userid).Result;
            var product = _context.Product.Where(p => p.Id == id).First();
            var image = _context.Image.Where(i => i.ProductId == id).First();
            var productSize = _context.ProductSize.Where(p => p.ProductId == id).ToList();
            List<Size> sizes = new List<Size>();
            foreach (var item in productSize)
            {
                var qr = _context.Sizes.Where(s => s.Id == item.SizeId).First();
                sizes.Add(qr);
            }
            var reviews = new ReviewsViewModel();
            reviews.UserId = userid;
            reviews.Avatar = user.Avatar;
            reviews.ProductId = product.Id;
            reviews.ProductName = product.Name;
            reviews.ImageProduct = image.Path;
            reviews.Sizes = sizes;
            return View(reviews);
        }

        // POST: ReviewsController/Create
        [HttpPost]
        [Authorize]
        [Route("reviews", Name = "reviews")]
        public async Task<ActionResult> Create(int id, ReviewsViewModel reviewsModel)
        {
            string fileName = "";
            if(reviewsModel.Image != null)
            {
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                fileName = Path.GetFileNameWithoutExtension(reviewsModel.Image.FileName);
                string extension = Path.GetExtension(reviewsModel.Image.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string filePath = Path.Combine(wwwRootPath + "/img/reviews/", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await reviewsModel.Image.CopyToAsync(fileStream);
                }
            }
            else
            {
                fileName = "";
            }
            var userid = _userManager.GetUserId(HttpContext.User);
            var reviews = new Reviews();
            reviews.Rate = reviewsModel.Rate;
            reviews.Content = reviewsModel.Content;
            reviews.SelectedSize = reviewsModel.Size;
            if (fileName == "")
            {
                reviews.Image = fileName;
            }
            else
            {
                reviews.Image = "/img/reviews/" + fileName;
            }
            reviews.ProductId = id;
            reviews.UserId = userid;
            _context.Add(reviews);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Product", new { id = reviews.ProductId });
        }
    }
}
