using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.ViewModels
{
    public class ChatViewModel
    {
        public DateTime CreatedAt { set; get; }
        public string UserName { set; get; }
        public List<ResponseMessageViewModel> Responses { set; get; }
        public string Request { set; get; }
        public string UserId { set; get; }
    }
}
