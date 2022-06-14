using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

            report.Revenue = RevenueByMonth(now.Month, now.Year);

            // Tính giá vốn

            report.CostPrice = CostPriceByMonth(now.Month, now.Year);

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

            report.Revenue = RevenueByMonth(reportView.SelectedMonth, reportView.SelectedYear);

            // Tính giá vốn

            report.CostPrice = CostPriceByMonth(report.SelectedMonth, report.SelectedYear);

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

                var revenue = RevenueByMonth(i, now.Year);

                // Vốn tháng thứ i

                var costPrice = CostPriceByMonth(i, now.Year);

                // Lợi nhuận tháng thứ i
                var profit = revenue - costPrice;
                report.ProfitList.Add(profit);

                // Doanh thu tháng thứ i
                report.RevenueList.Add(revenue);
            }

            // Tính doanh thu

            report.Revenue = RevenueByMonth(0, now.Year);

            // Tính giá vốn

            report.CostPrice = CostPriceByMonth(0, now.Year);

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

                var revenue = RevenueByMonth(i, reportView.SelectedYear);

                // Vốn tháng thứ i

                var costPrice = CostPriceByMonth(i, reportView.SelectedYear);

                // Lợi nhuận tháng thứ i
                var profit = revenue - costPrice;
                report.ProfitList.Add(profit);

                // Doanh thu tháng thứ i
                report.RevenueList.Add(revenue);
            }

            // Tính doanh thu

            report.Revenue = RevenueByMonth(0, reportView.SelectedYear);

            // Tính giá vốn

            report.CostPrice = CostPriceByMonth(0, report.SelectedYear);

            // Tính lợi nhuận
            report.Profit = (report.Revenue - report.CostPrice);

            return View(report);
        }

#region Helper

        private float RevenueByMonth(int month, int year)
        {
            var orders = month > 0 ? _context.Orders.Where(o => o.Created_date.Month == month && o.Created_date.Year == year && (o.Status < 4 && o.Status > 0)).ToList()
                                    : _context.Orders.Where(o => o.Created_date.Year == year && (o.Status < 4 && o.Status > 0)).ToList();
            float revenue = 0;
            foreach (var item in orders)
            {
                var orderdetails = _context.OrderDetail.Where(d => d.OrderId == item.Id).ToList();
                foreach (var i in orderdetails)
                {
                    var price = _context.OrderDetail.Where(d => d.Id == i.Id).First();
                    revenue += price.Price * price.Quantity;
                }
            }
            return revenue;
        }

        private float CostPriceByMonth(int month, int year)
        {
            var stocks = month > 0 ? _context.StockReceived.Where(s => s.Date.Month == month && s.Date.Year == year).ToList()
                                    : _context.StockReceived.Where(s => s.Date.Year == year).ToList();
            float costPrice = 0;
            foreach (var item in stocks)
            {
                var stockdetails = _context.StockReceivedDetail.Where(s => s.StockReceivedId == item.Id).ToList();
                foreach (var temp in stockdetails)
                {
                    var detail = _context.StockReceivedDetail.Where(r => r.Id == temp.Id).First();
                    costPrice += detail.Unit_price * detail.Quantity;
                }
            }
            return costPrice;
        }

#endregion
    }
}
