using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Address
    {
        public int Id { set; get; }
        public string Street { set; get; }
        public string Ward { set; get; }
        public string District { set; get; }
        public string City { set; get; }

        public virtual ICollection<AppUser> Users { get; set; }
    }
}
