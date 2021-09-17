﻿using System;
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
        public DateTime Year_SX { set; get; }
        public string Origin { set; get; }
        public string Trandemark { set; get; }
        public bool Active { set; get; }
        public int CategoryId { set; get; }
        public List<Image> Images { set; get; }
        public string ImageDefault { set; get; }
    }
}
