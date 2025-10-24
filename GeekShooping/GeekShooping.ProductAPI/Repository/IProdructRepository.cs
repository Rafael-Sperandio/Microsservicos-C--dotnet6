using GeekShooping.ProductAPI.Data.Dto;

namespace GeekShooping.ProductAPI.Repository
{
    public interface IProdructRepository
    {
        Task<IEnumerable<ProductDto>> GetAll();

        Task<ProductDto> GetById(long id);

        Task<ProductDto> Create(ProductDto dto);

        Task<ProductDto> Upddate(ProductDto dto);

        Task<bool> Delete(long id);




    }
}
