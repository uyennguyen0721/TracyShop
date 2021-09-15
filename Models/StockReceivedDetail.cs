using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class StockReceivedDetail
    {
        [Key]
        public int Id { set; get; }
        public int Quantity { set; get; }
        public float Unit_price { set; get; }
        public int StockReceivedId { set; get; }
        public int ProductId { set; get; }

        public virtual StockReceived StockReceived { set; get; }
        public virtual Product Product { set; get; }
    }
}
