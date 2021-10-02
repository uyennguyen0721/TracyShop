using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class StockViewModel
    {
        public int ProductId { set; get; }
        public string ProductName { set; get; }
        public int TotalQuantity { set; get; }
    }
}
