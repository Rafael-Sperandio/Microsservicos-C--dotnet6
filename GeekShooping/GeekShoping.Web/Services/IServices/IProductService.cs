using GeekShoping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeekShoping.Web.Services.IServices
{
    public interface IProductService
    {

        Task<ProductViewModel> GetProductById(long id);

        Task<IEnumerable<ProductViewModel>> GetAllProducts();

       Task<ProductViewModel> CreateProduct(ProductViewModel dto);
        Task<ProductViewModel> UpdateProduct(ProductViewModel dto);
        Task<bool> DeleteProductById(long id);

    }
}
