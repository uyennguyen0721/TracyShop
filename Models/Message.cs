using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Message
    {
        [Key]
        public int Id { set; get; }
        public string RequestMessage { set; get; }
        public string ResponseMessage { set; get; }
        
        public virtual ICollection<Chat> Chats { set; get; }
    }
}
