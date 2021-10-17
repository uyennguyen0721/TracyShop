using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class ReportViewModel
    {
        public float Revenue { set; get; }  // doanh thu
        public float Profit { set; get; }   // lợi nhuận
        public float CostPrice { set; get; }    // giá vốn
        public int SelectedMonth { set; get; }
        public int SelectedYear { set; get; }
        public List<int> Months { set; get; }
        public List<int> Years { set; get; }
        public List<float> ProfitList { set; get; } // Lợi nhuận của các tháng trong năm
        public List<float> RevenueList { set; get; }    // Doanh thu của các tháng trong năm
    }
}
