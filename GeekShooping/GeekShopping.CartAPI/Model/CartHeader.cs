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
        [Required]
        public string UserId;

        [Column("cupon_code")]
        [StringLength(500)]
        public string CuponCode { get; set; }

    }
}
