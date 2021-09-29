using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.ViewModels
{
    public class ProductsListViewModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public float Price { set; get; }
        public float PriceDiscounted { set; get; }
        public DateTime Year_SX { set; get; }
        public string Origin { set; get; }
        public string Trandemark { set; get; }
        public bool Active { set; get; }
        public int CategoryId { set; get; }
        public List<Image> Images { set; get; }
        public string ImageDefault { set; get; }
        public float TotalPrice { set; get; } // Tổng tiền khi mua x (số lượng) sản phẩm đó
        public int Count { set; get; }
        public List<Size> Sizes { set; get; }
        public List<Reviews> Reviews { set; get; }
        public int Promotion { set; get; }
        public int QuantityAvailable { set; get; } // Số lượng sẵn có
        public int QuantityPurchased { set; get; } // Số lượng mua
    }
}
