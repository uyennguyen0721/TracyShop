using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class District
    {
        [Key]
        public int Id { set; get; }
        [MaxLength(50)]
        public string Name { set; get; }
        public int CityId { set; get; }

        public virtual City City { set; get; }

        public virtual ICollection<Address> Addresses { set; get; }
    }
}
