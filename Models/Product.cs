using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Product
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public float Price { set; get; }
        public DateTime Year_SX { set; get; }
        public int? Quantity { set; get; }

        public virtual Trandemark Trandemark { set; get; }
        public virtual Category Category { set; get; }
        public virtual Promotion Promotion { set; get; }
        public virtual Size Size { set; get; }

        public virtual ICollection<Image> Images { set; get; }
        public virtual ICollection<Reviews> Reviews { set; get; }
        public virtual ICollection<StockReceivedDetail> StockReceivedDetails { set; get; }
        public virtual ICollection<OrderDetail> OrderDetails { set; get; }
    }
}
