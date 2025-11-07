using AutoMapper;
using GeekShooping.CartAPI.Data.Dto;
using GeekShooping.CartAPI.Model;
using GeekShooping.CartAPI.Model.Base;

namespace GeekShooping.CartAPI.Config.MappingConfig
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                config.CreateMap<CartDto, Cart>().ReverseMap();
                config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailDto, CartDetail>().ReverseMap();


            });
            return mappingConfig;
        }

    }
}
