using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;
using Microsoft.EntityFrameworkCore;
using TracyShop.Data;

namespace TracyShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Product> Products => _appDbContext.Product.Include(c => c.Category);

        public Product GetProductById(int productId) => _appDbContext.Product.FirstOrDefault(p => p.Id == productId);
    }
}
