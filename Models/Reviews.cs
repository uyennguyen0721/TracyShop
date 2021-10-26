using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Reviews
    {
        [Key]
        public int Id { set; get; }
        public int Rate { set; get; }

        [MaxLength(255)]
        public string Content { set; get; }
        public int SelectedSize { set; get; }

        [MaxLength(100)]
        public string Image { set; get; }
        public DateTime CreatedDate { set; get; } = DateTime.Now;

        public int ProductId { set; get; }
        public string UserId { set; get; }

        public virtual Product Product { set; get; }
        public virtual AppUser User { set; get; }
    }
}
