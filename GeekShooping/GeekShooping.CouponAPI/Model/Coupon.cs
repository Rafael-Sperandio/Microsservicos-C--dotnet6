
using GeekShooping.CouponAPI.Model.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.CouponAPI.Model
{
    [Table("coupon")]
    public class Coupon : BaseEntity
    {
        [Column("coupon_code")]
        [Required]
        [StringLength(60)]
        public string CouponCode {  get; set; }

        [Column("discount_amount")]
        [Required]
        public decimal DiscountAmount { get; set; }

        

    }
}
