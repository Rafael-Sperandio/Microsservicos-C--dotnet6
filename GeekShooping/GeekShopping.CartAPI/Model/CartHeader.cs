using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.CartAPI.Model.Base
{
    //TODO Nome Mudar
    //usario e desconto da compra
    //cartUserCupon
    [Table("cart_header")]
    public class CartHeader : BaseEntity
    {

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("coupon_code")]
        [StringLength(500)]
        public string CouponCode { get; set; }

    }
}
