using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Index()
        {
            var report = new ReportViewModel();
            DateTime now = DateTime.Now;

            // Lợi nhuận và doanh thu của các tháng trong năm
            report.ProfitList = new List<float>();
            report.RevenueList = new List<float>();

            for (int i = 1; i <= 12; i++)
            {
                // Doanh thu tháng thứ i
                var od = _context.Orders.Where(o => o.Created_date.Month == i && o.Created_date.Year == now.Year).ToList();
                float revenue = 0;
                foreach (var item in od)
                {
                    var odt = _context.OrderDetail.Where(d => d.OrderId == item.Id).ToList();
                    foreach (var j in odt)
                    {
                        revenue += _context.OrderDetail.Where(d => d.Id == j.Id).First().Price;
                    }
                }

                // Vốn tháng thứ i
                var st = _context.StockReceived.Where(s => s.Date.Month == i && s.Date.Year == now.Year).ToList();
                float costPrice = 0;
                foreach (var item in st)
                {
                    var std = _context.StockReceivedDetail.Where(s => s.StockReceivedId == item.Id).ToList();
                    foreach (var temp in std)
                    {
                        costPrice += _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First().Unit_price;
                    }
                }

                // Lợi nhuận tháng thứ i
                var profit = revenue - costPrice;
                report.ProfitList.Add(profit);

                // Doanh thu tháng thứ i
                report.RevenueList.Add(revenue);
            }

            // Tính doanh thu
            var orders = _context.Orders.Where(o => o.Created_date.Year == now.Year).ToList();
            report.Revenue = 0;
            foreach (var item in orders)
            {
                var orderdetails = _context.OrderDetail.Where(d => d.OrderId == item.Id).ToList();
                foreach (var i in orderdetails)
                {
                    report.Revenue += _context.OrderDetail.Where(d => d.Id == i.Id).First().Price;
                }
            }

            // Tính giá vốn
            var stocks = _context.StockReceived.Where(s => s.Date.Year == now.Year && s.Date.Month == now.Month).ToList();
            report.CostPrice = 0;
            foreach (var item in stocks)
            {
                var stockdetails = _context.StockReceivedDetail.Where(s => s.StockReceivedId == item.Id).ToList();
                foreach (var temp in stockdetails)
                {
                    report.CostPrice += _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First().Unit_price;
                }
            }

            // Tính lợi nhuận
            report.Profit = report.RevenueList[now.Month - 1] - report.CostPrice;

            // Tính số lượng tồn kho
            var products = _context.Product.ToList();
            var totalStock = 0;
            foreach (var product in products)
            {
                var productSizes = _context.ProductSize.Where(p => p.ProductId == product.Id).ToList();
                foreach (var item in productSizes)
                {
                    totalStock += item.Quantity;
                }
            }

            // Tính số lượng nhập kho
            var received = _context.StockReceivedDetail.ToList();
            var totalReceived = 0;
            foreach(var item in received)
            {
                totalReceived += item.Quantity;
            }

            // tính phần trăm hàng tồn kho
            if(totalStock == 0 || totalReceived == 0)
            {
                ViewBag.Stock = 0;
            }
            else
            {
                ViewBag.Stock = totalStock / totalReceived * 100;
            }

            // Tính số lượng phản hồi đánh giá
            ViewBag.Review = _context.Reviews.ToList().Count;

            // Phần trăm đánh giá theo sao
            var star = _context.Reviews.Where(p => p.Rate != 0).ToList();
            
            if(star.Count != 0)
            {
                float oneStar = 0, twoStar = 0, threeStar = 0, fourStar = 0, fiveStar = 0;
                foreach (var item in star)
                {
                    if (item.Rate == 1)
                        oneStar++;
                    else if (item.Rate == 2)
                        twoStar++;
                    else if (item.Rate == 3)
                        threeStar++;
                    else if (item.Rate == 4)
                        fourStar++;
                    else if (item.Rate == 5)
                        fiveStar++;
                }
                ViewBag.OneStar = oneStar / star.Count * 100;
                ViewBag.TwoStar = twoStar / star.Count * 100;
                ViewBag.ThreeStar = threeStar / star.Count * 100;
                ViewBag.FourStar = fourStar / star.Count * 100;
                ViewBag.FiveStar = fiveStar / star.Count * 100;
            }
            else
            {
                ViewBag.OneStar = ViewBag.TwoStar = ViewBag.ThreeStar = ViewBag.FourStar = ViewBag.FiveStar = 0;
            }

            // Khuyến mãi
            
            if(_context.Promotion.Where(p => p.StartedDate <= DateTime.Now && p.EndDate >= DateTime.Now).ToList().Count != 0)
            {
                var promotion = _context.Promotion.Where(p => p.StartedDate <= DateTime.Now && p.EndDate >= DateTime.Now).ToList();
                ViewBag.Promotion = promotion.First().percent * 100;
            }
            else
            {
                ViewBag.Promotion = 0;
            }

            return View(report);
        }
    }
}
