using AutoMapper;
using GeekShooping.ProductAPI.Data.Dto;
using GeekShooping.ProductAPI.Model;

namespace GeekShooping.ProductAPI.Config.MappingConfig
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto > ();
            });
            return mappingConfig;
        }

    }
}
