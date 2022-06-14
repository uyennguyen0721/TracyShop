using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TracyShop.Data;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Index(int? id)
        {
            if(id == null)
            {
                ViewBag.Id = true;
                if (_context.Reviews.ToList().Count != 0)
                {
                    ViewBag.Message = "";
                    var reviews = _context.Reviews.ToList().OrderByDescending(p => p.CreatedDate).OrderByDescending(p => p.Rate);
                    return View(reviews);
                }
                else
                {
                    ViewBag.Message = "Chưa có phản hồi.";
                    return View();
                }
            }
            else
            {
                ViewBag.Id = false;
                if (_context.Reviews.Where(r => r.ProductId == id).ToList().Count != 0)
                {
                    ViewBag.Message = "";
                    var review = _context.Reviews.Where(r => r.ProductId == id).ToList().OrderByDescending(p => p.CreatedDate).OrderByDescending(p => p.Rate);
                    return View(review);
                }
                else
                {
                    ViewBag.Message = "Chưa có phản hồi.";
                    return View();
                }
            }
        }
    }
}
