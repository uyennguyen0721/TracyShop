using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.Repository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
    }
}
