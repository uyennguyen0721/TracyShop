using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.ViewModels;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("/admin/report-by-month", Name ="report-by-month")]
        public IActionResult ReportByMonth()
        {
            var report = new ReportViewModel();
            DateTime now = DateTime.Now;

            // Hiển thị lên giao diện
            report.Months = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                report.Months.Add(i);
            }
            report.Years = new List<int>();
            for(int i = 1970; i <= now.Year; i++)
            {
                report.Years.Add(i);
            }
            report.SelectedMonth = now.Month;
            report.SelectedYear = now.Year;

            // Tính doanh thu
            var orders = _context.Orders.Where(o => o.Created_date.Month == now.Month && o.Created_date.Year == now.Year).ToList();
            report.Revenue = 0;
            foreach (var item in orders)
            {
                var orderdetails = _context.OrderDetail.Where(d => d.OrderId == item.Id).ToList();
                foreach (var i in orderdetails)
                {
                    var price = _context.OrderDetail.Where(d => d.Id == i.Id).First();
                    report.Revenue += (price.Price * price.Quantity);
                }
            }

            // Tính giá vốn
            var stocks = _context.StockReceived.Where(s => s.Date.Month == now.Month && s.Date.Year == now.Year).ToList();
            report.CostPrice = 0;
            foreach (var item in stocks)
            {
                var stockdetails = _context.StockReceivedDetail.Where(s => s.StockReceivedId == item.Id).ToList();
                foreach (var temp in stockdetails)
                {
                    var detail = _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First();
                    report.CostPrice += detail.Unit_price * detail.Quantity;
                }
            }

            // Tính lợi nhuận
            report.Profit = report.Revenue - report.CostPrice;

            return View(report);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("/admin/report-by-month", Name = "report-by-month")]
        public IActionResult ReportByMonth(ReportViewModel reportView)
        {
            var report = new ReportViewModel();

            // Hiển thị lên giao diện
            report.Months = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                report.Months.Add(i);
            }
            report.Years = new List<int>();
            for (int i = 1970; i <= DateTime.Now.Year; i++)
            {
                report.Years.Add(i);
            }
            report.SelectedMonth = reportView.SelectedMonth;
            report.SelectedYear = reportView.SelectedYear;

            // Tính doanh thu
            var orders = _context.Orders.Where(o => o.Created_date.Month == reportView.SelectedMonth && o.Created_date.Year == reportView.SelectedYear).ToList();
            report.Revenue = 0;
            foreach (var item in orders)
            {
                var orderdetails = _context.OrderDetail.Where(d => d.OrderId == item.Id).ToList();
                foreach (var i in orderdetails)
                {
                    var price = _context.OrderDetail.Where(d => d.Id == i.Id).First();
                    report.Revenue += price.Price * price.Quantity;
                }
            }

            // Tính giá vốn
            var stocks = _context.StockReceived.Where(s => s.Date.Month == report.SelectedMonth && s.Date.Year == report.SelectedYear).ToList();
            report.CostPrice = 0;
            foreach (var item in stocks)
            {
                var stockdetails = _context.StockReceivedDetail.Where(s => s.StockReceivedId == item.Id).ToList();
                foreach (var temp in stockdetails)
                {
                    var detail = _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First();
                    report.CostPrice += detail.Unit_price * detail.Quantity;
                }
            }

            // Tính lợi nhuận
            report.Profit = (report.Revenue - report.CostPrice);

            return View(report);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("/admin/report-by-year", Name = "report-by-year")]
        public IActionResult ReportByYear()
        {
            var report = new ReportViewModel();
            DateTime now = DateTime.Now;

            // Hiển thị lên giao diện
            report.Years = new List<int>();
            for (int i = 1970; i <= now.Year; i++)
            {
                report.Years.Add(i);
            }
            report.SelectedYear = now.Year;

            // Lợi nhuận và doanh thu của các tháng trong năm
            report.ProfitList = new List<float>();
            report.RevenueList = new List<float>();

            for(int i = 1; i <= 12; i++)
            {
                // Doanh thu tháng thứ i
                var od = _context.Orders.Where(o => o.Created_date.Month == i && o.Created_date.Year == now.Year).ToList();
                float revenue = 0;
                foreach(var item in od)
                {
                    var odt = _context.OrderDetail.Where(d => d.OrderId == item.Id).ToList();
                    foreach (var j in odt)
                    {
                        var price = _context.OrderDetail.Where(d => d.Id == j.Id).First();
                        revenue += price.Price * price.Quantity;
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
                        var detail = _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First();
                        costPrice += detail.Unit_price * detail.Quantity;
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
                    var price = _context.OrderDetail.Where(d => d.Id == i.Id).First();
                    report.Revenue += price.Price * price.Quantity;
                }
            }

            // Tính giá vốn
            var stocks = _context.StockReceived.Where(s => s.Date.Year == now.Year).ToList();
            report.CostPrice = 0;
            foreach (var item in stocks)
            {
                var stockdetails = _context.StockReceivedDetail.Where(s => s.StockReceivedId == item.Id).ToList();
                foreach (var temp in stockdetails)
                {
                    var detail = _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First();
                    report.CostPrice += detail.Unit_price * detail.Quantity;
                }
            }

            // Tính lợi nhuận
            report.Profit = report.Revenue - report.CostPrice;

            return View(report);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("/admin/report-by-year", Name = "report-by-year")]
        public IActionResult ReportByYear(ReportViewModel reportView)
        {
            var report = new ReportViewModel();

            // Hiển thị lên giao diện
            report.Years = new List<int>();
            for (int i = 1970; i <= DateTime.Now.Year; i++)
            {
                report.Years.Add(i);
            }
            report.SelectedYear = reportView.SelectedYear;

            // Lợi nhuận và doanh thu của các tháng trong năm
            report.ProfitList = new List<float>();
            report.RevenueList = new List<float>();
            for (int i = 1; i <= 12; i++)
            {
                // Doanh thu tháng thứ i
                var od = _context.Orders.Where(o => o.Created_date.Month == i && o.Created_date.Year == reportView.SelectedYear).ToList();
                float revenue = 0;
                foreach (var item in od)
                {
                    var odt = _context.OrderDetail.Where(d => d.OrderId == item.Id).ToList();
                    foreach (var j in odt)
                    {
                        var price = _context.OrderDetail.Where(d => d.Id == j.Id).First();
                        revenue += price.Price * price.Quantity;
                    }
                }

                // Vốn tháng thứ i
                var st = _context.StockReceived.Where(s => s.Date.Month == i && s.Date.Year == reportView.SelectedYear).ToList();
                float costPrice = 0;
                foreach (var item in st)
                {
                    var std = _context.StockReceivedDetail.Where(s => s.StockReceivedId == item.Id).ToList();
                    foreach (var temp in std)
                    {
                        var detail = _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First();
                        costPrice += detail.Unit_price * detail.Quantity;
                    }
                }

                // Lợi nhuận tháng thứ i
                var profit = revenue - costPrice;
                report.ProfitList.Add(profit);

                // Doanh thu tháng thứ i
                report.RevenueList.Add(revenue);
            }

            // Tính doanh thu
            var orders = _context.Orders.Where(o => o.Created_date.Year == reportView.SelectedYear).ToList();
            report.Revenue = 0;
            foreach (var item in orders)
            {
                var orderdetails = _context.OrderDetail.Where(d => d.OrderId == item.Id).ToList();
                foreach (var i in orderdetails)
                {
                    var price = _context.OrderDetail.Where(d => d.Id == i.Id).First();
                    report.Revenue += price.Price * price.Quantity;
                }
            }

            // Tính giá vốn
            var stocks = _context.StockReceived.Where(s => s.Date.Year == report.SelectedYear).ToList();
            report.CostPrice = 0;
            foreach (var item in stocks)
            {
                var stockdetails = _context.StockReceivedDetail.Where(s => s.StockReceivedId == item.Id).ToList();
                foreach (var temp in stockdetails)
                {
                    var detail = _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First();
                    report.CostPrice += detail.Unit_price * detail.Quantity;
                }
            }

            // Tính lợi nhuận
            report.Profit = (report.Revenue - report.CostPrice);

            return View(report);
        }
    }
}
