using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { set; get; }
        public int Quantity { set; get; }
        public float Unit_price { set; get; }
        public int OrderId { set; get; }
        public int ProductId { set; get; }

        public virtual Order Order { set; get; }
        public virtual Product Product { set; get; }
    }
}
