using GeekShooping.CouponAPI.Data.Dto;

namespace GeekShooping.CouponAPI.Repository
{
    public interface ICouponRepository
    {

        Task<CouponDto> GetCouponByCouponCode(string couponCode);
/*
        Task<CouponDto> CreateAsync(CouponDto coupon); 
//*/
    }
}
