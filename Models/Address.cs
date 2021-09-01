using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Address
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(50)]
        public string SpecificAddress { set; get; }

        [MaxLength(25)]
        public string District { set; get; }

        [MaxLength(25)]
        public string City { set; get; }

        public virtual AppUser User { get; set; }
    }
}
