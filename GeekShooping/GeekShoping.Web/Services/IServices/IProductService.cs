using GeekShoping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeekShoping.Web.Services.IServices
{
    public interface IProductService
    {

        Task<ProductViewModel> GetProductById(long id,string token);

        Task<IEnumerable<ProductViewModel>> GetAllProducts(string token);

       Task<ProductViewModel> CreateProduct(ProductViewModel dto, string token);
        Task<ProductViewModel> UpdateProduct(ProductViewModel dto, string token);
        Task<bool> DeleteProductById(long id, string token);

    }
}
