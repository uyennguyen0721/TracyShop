using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        [Route("/customer")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Where(x => x.UserRole.Id == 3).ToListAsync());
        }

        // GET: Admin/Customer/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id.Contains(id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult OrderView(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.Orders.Where(o => o.UserId.Contains(id)).ToList();

            if (order.Count == 0)
            {
                ViewBag.Message = "Chưa có đơn hàng nào được tạo.";
                return View();
            }
            else
            {
                ViewBag.Message = "";
                return View(order);
            }
        }
    }
}
