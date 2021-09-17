using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;

namespace TracyShop.Services
{
    public class ProductsService
    {
        private readonly AppDbContext _context;

        public ProductsService(AppDbContext context)
        {
            _context = context;
        }
        public Product[] Get(string search)
        {
            var s = search.ToLower();
            return _context.Product.Where(p =>
                p.Name.ToLower().Contains(s) ||
                p.Category.Name.ToLower().Contains(s) ||
                p.Trandemark.ToLower().Contains(s) ||
                p.Origin.Contains(s)
            ).ToArray();
        }
    }
}
