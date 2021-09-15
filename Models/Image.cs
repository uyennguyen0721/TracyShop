using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Image
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(100)]
        public string Path { set; get; }
        public int ProductId { set; get; }

        public virtual Product Product { set; get; }
    }
}
