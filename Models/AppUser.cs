using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TracyShop.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(100)]
        public string Name { set; get; }

        [MaxLength(255)]
        public string Avatar { set; get; }
        public DateTime? Birthday { set; get; }

        [MaxLength(10)]
        public string Gender { set; get; }
        public bool Is_active { set; get; }
        public DateTime Joined_date { set; get; }

        public virtual Address Address { set; get; }
        public virtual UserRole UserRole { set; get; }

        public virtual ICollection<Reviews> Reviews { set; get; }
        public virtual ICollection<StockReceived> StockReceiveds { set; get; }
        public virtual ICollection<Cart> Carts { set; get; }

    }
}
