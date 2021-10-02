using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;

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

        [Authorize(Roles = "Admin, Employee")]
        [Route("/customer")]
        public async Task<IActionResult> Index()
        {
            var userRole = _context.UserRoles.Where(u => u.RoleId.Contains("3")).ToList();
            List<AppUser> users = new List<AppUser>();
            foreach (var item in userRole)
            {
                var user = await _context.Users.Where(u => u.Id.Contains(item.UserId)).FirstAsync();
                users.Add(user);
            }
            return View(users);
        }

        // GET: Admin/Customer/Details/5
        [Authorize(Roles = "Admin, Employee")]
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

        [Authorize(Roles = "Admin, Employee")]
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
