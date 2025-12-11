using GeekShooping.CartAPI.Data.Dto;
using GeekShopping.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekShopping.CartAPI.Messages
{
    public class CheckoutHeaderDto : BaseMessage
    {
/*        public long Id { get; set; }*/
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMothYear { get; set; }

        public int CartTotalItens { get; set; } //= 0;
        public IEnumerable<CartDetailDto> CartDetails { get; set; } 
            //= new List<CartDetailDto>();
    }
}
