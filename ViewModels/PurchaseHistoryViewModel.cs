using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.ViewModels
{
    public class PurchaseHistoryViewModel
    {
        public int OrderId { set; get; }
        public DateTime OrderDate { set; get; }
        public string UserId { set; get; }
        public float TotalPrice { set; get; }
        public int Status { set; get; }
        public List<OrderDetail> OrderDetails { set; get; }
    }
}
