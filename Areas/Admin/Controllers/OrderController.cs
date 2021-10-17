using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Index()
        {
            if(_context.Orders.ToList().Count == 0)
            {
                ViewBag.Message = "Chưa có đơn hàng nào được tạo.";
                return View();
            }
            else
            {
                ViewBag.Message = "";
                var order = _context.Orders.ToList().OrderByDescending(p => p.Created_date);
                return View(order);
            }
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Details(int id)
        {
            var orderDetail = _context.OrderDetail.Where(o => o.OrderId == id).ToList();
            return View(orderDetail);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        [Route("/admin/checked-order", Name = "checked-order")]
        public IActionResult CheckedOrder()
        {
            if (_context.Orders.Where(p => p.Is_check == false).ToList().Count == 0)
            {
                ViewBag.Message = "Không tìm thấy đơn hàng nào chưa duyệt.";
                return View();
            }
            else
            {
                ViewBag.Message = "";
                var order = _context.Orders.Where(p => p.Is_check == false).ToList();
                return View(order);
            }
        }

        [HttpPost]
        [Route("/admin/checked-order", Name = "checked-order")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> CheckedOrder(int id)
        {
            var order = _context.Orders.Where(p => p.Id == id).First();
            order.Is_check = true;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CheckedOrder));
        }
    }
}
