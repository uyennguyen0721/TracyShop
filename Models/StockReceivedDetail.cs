﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class StockReceivedDetail
    {
        public int Id { set; get; }
        public int Quantity { set; get; }
        public float Unit_price { set; get; }

        public virtual StockReceived StockReceived { set; get; }
        public virtual Product Product { set; get; }
    }
}
