using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Category
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public virtual ICollection<Product> Products { set; get; }
    }
}
