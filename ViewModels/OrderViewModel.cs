using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.ViewModels
{
    public class OrderViewModel
    {
        public int Id { set; get; }
        public DateTime Created_date { set; get; }
        public bool Is_check { set; get; }
        public bool Is_pay { set; get; }
        public int ProductId { set; get; }
        public int SelectedSize { set; get; }
        public List<Cart> Carts { set; get; }
        public string UserId { set; get; }
        public string Address { set; get; }
        public string PhoneNumber { set; get; }
        public float TotalPrice { set; get; }
        public int CountQuantity { set; get; }
        public double ShoppingFee { set; get; }
        public int PaymentMenthodId { set; get; }
        public List<PaymentMenthod> PaymentMenthods { set; get; }
    }
}
