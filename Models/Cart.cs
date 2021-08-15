﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Cart
    {
        [Key]
        public int Id { set; get; }

        public int Quantity { set; get; }
        public float UnitPrice { set; get; }

        public virtual AppUser User { set; get; }
        public virtual Product Product { set; get; }
    }
}
