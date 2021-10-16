using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.ViewModels
{
    public class ProductStockItem
    {
        public List<Product> Products { set; get; }
        public List<Size> Sizes { set; get; }
        public int SelectedPro { set; get; }
        public int SelectedSize { set; get; }
        public int Quantity { set; get; }
        public float UnitPrice { set; get; }
    }
}
