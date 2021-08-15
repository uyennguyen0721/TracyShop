using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Product
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(100)]
        public string Name { set; get; }

        [MaxLength(255)]
        public string Description { set; get; }
        public float Price { set; get; }
        public DateTime Year_SX { set; get; }
        public int? Quantity { set; get; }
        public bool Active { set; get; }

        public virtual Trandemark Trandemark { set; get; }
        public virtual Category Category { set; get; }
        public virtual Promotion Promotion { set; get; }
        public virtual Size Size { set; get; }

        public virtual ICollection<Image> Images { set; get; }
        public virtual ICollection<Reviews> Reviews { set; get; }
        public virtual ICollection<StockReceivedDetail> StockReceivedDetails { set; get; }
        public virtual ICollection<OrderDetail> OrderDetails { set; get; }
        public virtual ICollection<Cart> Carts { set; get; }
    }
}
