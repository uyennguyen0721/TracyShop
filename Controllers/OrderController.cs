using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.Repository;
using TracyShop.ViewModels;

namespace TracyShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoginRepository _loginRepository;

        public OrderController(ILogger<OrderController> logger, AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, ILoginRepository loginRepository)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _loginRepository = loginRepository;
        }

        // GET: OrderController/Create
        [HttpGet]
        [Authorize]
        [Route("/orders", Name = "orders")]
        public IActionResult Order()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            AppUser user = _userManager.FindByIdAsync(userid).Result;

            // Lấy địa chỉ
            Address address = new Address();
            try
            {
                address = _context.Address.Where(a => a.UserId == userid).ToList().First();
            }
            catch
            {
                address = null;
            }
            // Lấy phương thức thanh toán
            var paymentMenthods = _context.PaymentMenthod.ToList();

            // Lấy tất cả các sản phẩm trong card chưa thanh toán (chưa mua)
            var carts = _context.Carts.Where(c => c.UserId == userid && c.IsBuy == false).ToList();

            // Hiển thị lên view

            var orderView = new OrderViewModel();
            orderView.UserId = userid;
            if(address != null)
            {
                orderView.Address = String.Format("{0}, {1}, {2}", address.SpecificAddress, address.District, address.City);
            }
            else
            {
                orderView.Address = null;
            }
            orderView.PhoneNumber = user.PhoneNumber;
            if(user.PhoneNumber == null)
            {
                ViewBag.Message = "Thêm số điện thoại";
            }

            var products = new List<ProductsListViewModel>();
            float totalPrice = 0;
            int countQuantity = 0;
            foreach (var pr in carts)
            {
                totalPrice += pr.UnitPrice * (1 - pr.Promotion) * pr.Quantity;
                countQuantity += pr.Quantity;
            }
            orderView.Carts = carts;
            orderView.TotalPrice = totalPrice;
            orderView.CountQuantity = countQuantity;
            orderView.PaymentMenthods = paymentMenthods;
            orderView.ShoppingFee = 20000;

            ViewBag.Content = "";

            return View(orderView);
        }

        // POST: OrderController/Create
        [HttpPost]
        [Authorize]
        [Route("/orders", Name = "orders")]
        public async Task<IActionResult> Order(OrderViewModel orderView)
        {
            try
            {
                var order = new Order();
                order.PaymentMenthodId = orderView.PaymentMenthodId;
                order.ShoppingFee = 20000;
                order.UserId = _userManager.GetUserId(HttpContext.User);
                _context.Add(order);
                await _context.SaveChangesAsync();

                Task.Delay(3000).Wait();

                var cart = _context.Carts.Where(c => c.UserId == order.UserId && c.IsBuy == false).ToList();
                foreach (var item in cart)
                {
                    item.IsBuy = true;
                    _context.Update(item);
                }

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.OrderId = _context.Orders.OrderBy(x => x.Id).Last().Id;
                    orderDetail.ProductId = item.ProductId;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.SelectedSize = item.SelectedSize;
                    orderDetail.Price = item.UnitPrice * (1 - item.Promotion);
                    orderDetail.Promotion = item.Promotion;
                    _context.Add(orderDetail);

                    //Task.Delay(1000).Wait();

                    var query = _context.ProductSize.Where(p => p.ProductId == item.ProductId && p.SizeId == item.SelectedSize).First();
                    query.Quantity -= item.Quantity;
                    _context.Update(query);

                    //Task.Delay(1000).Wait();
                }
                await _context.SaveChangesAsync();

                Task.Delay(3000).Wait();

                ViewBag.Content = "Mua hàng";
                ViewBag.News = true;
                ViewBag.Class = "alert alert-success";
                ViewBag.Message = "Đặt hàng thành công!";

                return View();
            }
            catch
            {
                ViewBag.Content = "Mua hàng";
                ViewBag.News = false;
                ViewBag.Class = "alert alert-danger";
                ViewBag.Message = "Rất tiếc, quá trình đặt hàng của bạn chưa thành công. Vui lòng thử lại!";
                return View();
            }
        }
    }
}
