using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class UserRole
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public virtual ICollection<AppUser> Users { set; get; }
    }
}
