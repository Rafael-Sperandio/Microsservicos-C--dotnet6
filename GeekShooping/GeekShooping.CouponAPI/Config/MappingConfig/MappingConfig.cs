using AutoMapper;
using GeekShooping.CouponAPI.Data.Dto;
using GeekShooping.CouponAPI.Model;
using GeekShooping.CouponAPI.Model.Base;

namespace GeekShooping.CouponAPI.Config.MappingConfig
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>().ReverseMap();


            });
            return mappingConfig;
        }

    }
}
