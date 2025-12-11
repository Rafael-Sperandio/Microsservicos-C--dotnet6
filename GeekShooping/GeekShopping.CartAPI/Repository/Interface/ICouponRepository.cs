using GeekShopping.CartAPI.Data.Dto;
using System.Threading.Tasks;

namespace GeekShopping.CartAPI.Repository.Interface
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCoupon(string couponCode, string token);
    }
}
