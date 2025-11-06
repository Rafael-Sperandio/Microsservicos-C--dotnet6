using AutoMapper;
using GeekShooping.CartAPI.Data.Dto;
using GeekShooping.CartAPI.Model;

namespace GeekShooping.CartAPI.Config.MappingConfig
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            var mappingConfig = new MapperConfiguration(config =>
            {
/*                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto > ();*/
            });
            return mappingConfig;
        }

    }
}
