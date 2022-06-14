using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            order.Status += 1;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CheckedOrder));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        [Route("/admin/order-status", Name = "order-status")]
        public IActionResult UpdateStatus()
        {
            if (_context.Orders.Where(p => p.Is_check == true).ToList().Count == 0)
            {
                ViewBag.Message = "Chưa có đơn hàng nào.";
                return View();
            }
            else
            {
                ViewBag.Message = "";
                var orders = _context.Orders;
                ViewBag.WaitingForGetting = orders.Where(p => p.Status == 1).ToList().Count;
                ViewBag.Delivering = orders.Where(p => p.Status == 2).ToList().Count;
                ViewBag.Received = orders.Where(p => p.Status == 3).ToList().Count;
                ViewBag.Detroyed = orders.Where(p => p.Status == 4).ToList().Count;
                return View(orders.ToList());
            }
        }
        [HttpPost]
        [Route("/admin/order-status", Name = "order-status")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var order = _context.Orders.Where(p => p.Id == id).First();
            order.Status += 1;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("UpdateStatus", "Order");
        }
    }
}
