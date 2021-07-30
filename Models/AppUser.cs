using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TracyShop.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { set; get; }
        public string Avatar { set; get; }
        public DateTime Birthday { set; get; }
        public string Gender { set; get; }
        public bool Is_active { set; get; }
        public DateTime Joined_date { set; get; }

        public virtual Address Address { set; get; }
        public virtual UserRole UserRole { set; get; }

        public virtual ICollection<Reviews> Reviews { set; get; }
        public virtual ICollection<StockReceived> StockReceiveds { set; get; }

    }
}
