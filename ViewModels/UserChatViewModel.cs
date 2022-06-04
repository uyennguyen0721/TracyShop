using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class UserChatViewModel
    {
        public string Id { set; get; }
        public string UserName { set; get; }
        public string Avatar { set; get; }
        public bool IsSeen { set; get; }
        public string LastMessage { set; get; }
    }
}
