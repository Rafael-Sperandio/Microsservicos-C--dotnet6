using GeekShoping.Web.Models;
using GeekShopping.Web.Models;
using System.Threading.Tasks;

namespace GeekShoping.Web.Services.IServices
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartByUserId(string userId, string token);
        Task<CartViewModel> AddItemToCart(CartViewModel cart, string token);
        Task<CartViewModel> UpdateCart(CartViewModel cart, string token);
        Task<bool> DeleteFromCart(long cartId, string token);

        Task<bool> ApplyCoupon(CartViewModel cart, string token);
        Task<bool> RemoveCoupon(string userId, string token);
        Task<bool> ClearCart(string userId, string token);

        Task<CartHeaderViewModel> Checkout(CartHeaderViewModel cartHeader, string token);
     }
}
