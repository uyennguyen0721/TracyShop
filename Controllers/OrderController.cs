using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.Repository;
using TracyShop.ViewModels;
using PayPal.Core;
using PayPal.v1.Payments;
using Address = TracyShop.Models.Address;
using Order = TracyShop.Models.Order;
using BraintreeHttp;
using HttpResponse = BraintreeHttp.HttpResponse;

namespace TracyShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoginRepository _loginRepository;
        private readonly string _clientId;
        private readonly string _secretKey;

        public double TyGiaUSD = 22759;

        public OrderController(ILogger<OrderController> logger, AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, ILoginRepository loginRepository, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _loginRepository = loginRepository;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
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
            if(orderView.PaymentMenthodId == 1)
            {
                try
                {
                    var order = new Order();
                    order.PaymentMenthodId = orderView.PaymentMenthodId;
                    order.ShoppingFee = 20000;
                    order.UserId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(order);
                    await _context.SaveChangesAsync();

                    Task.Delay(200).Wait();

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

                        var query = _context.ProductSize.Where(p => p.ProductId == item.ProductId && p.SizeId == item.SelectedSize).First();
                        query.Quantity -= item.Quantity;
                        _context.Update(query);
                    }
                    await _context.SaveChangesAsync();

                    Task.Delay(300).Wait();

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
            else
            {
                var environment = new SandboxEnvironment(_clientId, _secretKey);
                var client = new PayPalHttpClient(environment);

                #region Create Paypal Order
                var itemList = new ItemList()
                {
                    Items = new List<Item>()
                };

                var cart = _context.Carts.Where(c => c.UserId == _userManager.GetUserId(HttpContext.User) && c.IsBuy == false).ToList();
                float totalPrice = 0;
                foreach (var item in cart)
                {
                    totalPrice += item.Quantity * item.UnitPrice * (1 - item.Promotion);
                }
                var shipping = Math.Round(20000 / TyGiaUSD, 2);
                var total = Math.Round(totalPrice / TyGiaUSD, 2) + shipping;
                var subTotal = Math.Round(totalPrice / TyGiaUSD, 2);
                foreach (var item in cart)
                {
                    itemList.Items.Add(new Item()
                    {
                        Name = _context.Product.Where(p => p.Id == item.ProductId).First().Name,
                        Currency = "USD",
                        Price = Math.Round(item.UnitPrice / TyGiaUSD, 2).ToString(),
                        Quantity = item.Quantity.ToString(),
                        Sku = "sku",
                        Tax = "0"
                    });
                }
                #endregion

                var paypalOrderId = DateTime.Now.Ticks;
                var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                var payment = new Payment()
                {
                    Intent = "sale",
                    Transactions = new List<Transaction>()
                    {
                        new Transaction()
                        {
                            Amount = new Amount()
                            {
                                Total = total.ToString(),
                                Currency = "USD",
                                Details = new AmountDetails
                                {
                                    Tax = "0",
                                    Shipping = shipping.ToString(),
                                    Subtotal = subTotal.ToString()
                                }
                            },
                            ItemList = itemList,
                            Description = $"Invoice #{paypalOrderId}",
                            InvoiceNumber = paypalOrderId.ToString()
                        }
                    },
                    RedirectUrls = new RedirectUrls()
                    {
                        CancelUrl = $"{hostname}/order/checkout-fail",
                        ReturnUrl = $"{hostname}/order/checkout-success"
                    },
                    Payer = new Payer()
                    {
                        PaymentMethod = "paypal"
                    }
                };

                PaymentCreateRequest request = new PaymentCreateRequest();
                request.RequestBody(payment);

                try
                {
                    HttpResponse response = await client.Execute(request);
                    var statusCode = response.StatusCode;
                    Payment result = response.Result<Payment>();

                    var links = result.Links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        LinkDescriptionObject lnk = links.Current;
                        if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.Href;
                        }
                    }

                    return Redirect(paypalRedirectUrl);
                }
                catch (HttpException httpException)
                {
                    var statusCode = httpException.StatusCode;
                    var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                    //Process when Checkout with Paypal fails
                    return Redirect("/order/checkout-fail");
                }
            }
        }

        [Route("/order/checkout-fail")]
        public IActionResult CheckoutFail()
        {
            return View();
        }

        [Route("/order/checkout-success")]
        public async Task<IActionResult> CheckoutSuccess()
        {
            var order = new Order();
            order.PaymentMenthodId = 2;
            order.ShoppingFee = 20000;
            order.UserId = _userManager.GetUserId(HttpContext.User);
            _context.Add(order);
            await _context.SaveChangesAsync();

            Task.Delay(500).Wait();

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

                var query = _context.ProductSize.Where(p => p.ProductId == item.ProductId && p.SizeId == item.SelectedSize).First();
                query.Quantity -= item.Quantity;
                _context.Update(query);
            }
            await _context.SaveChangesAsync();

            Task.Delay(500).Wait();

            return View();
        }
    }
}
