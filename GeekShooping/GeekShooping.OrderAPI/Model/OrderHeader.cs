using GeekShopping.OrderAPI.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.OrderAPI.Model.Base
{
    [Table("order_header")]
    public class OrderHeader : BaseEntity
    {
        [Column("user_id")]
        public string UserId { get; set; }

        [Column("coupon_code")]
        public string CouponCode { get; set; }

        [Column("purchase_amount")]
        public decimal PurchaseAmount { get; set; }

        [Column("discount_amount")]
        public decimal DiscountAmount { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("purchase_date")]
        public DateTime DateTime { get; set; }

        [Column("order_time")]
        public DateTime OrderTime { get; set; }

        [Column("phone_number")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("card_number")]
        public string CardNumber { get; set; }

        [Column("cvv")]
        public string CVV { get; set; }

        [Column("expiry_month_year")]
        public string ExpiryMonthYear { get; set; }

        [Column("total_itens")]
        public int CartTotalItens { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        [Column("payment_status")]
        public bool PaymentStatus { get; set; }



        /// <summary>
        /// Adiciona vários itens ao pedido com base nos dados do carrinho,
        /// criando automaticamente instâncias de <see cref="OrderDetail"/> e
        /// atualizando o total de itens.
        /// </summary>
        /// <param name="cartItems">
        /// Lista de itens vindos do carrinho, contendo informações de produto e quantidade.
        /// </param>
        public void AddOrderItems(IEnumerable<CartDetaildto> cartItems)
        {
            if (cartItems == null) return;

            foreach (var item in cartItems)
            {
                var detail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Count = item.Count
                };

                CartTotalItens += item.Count;
                OrderDetails.Add(detail);
            }
        }
    }
}
