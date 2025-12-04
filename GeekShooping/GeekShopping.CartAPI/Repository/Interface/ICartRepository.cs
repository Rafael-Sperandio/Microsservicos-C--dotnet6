using GeekShooping.CartAPI.Data.Dto;

namespace GeekShopping.CartAPI.Repository.Interface
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserId(String userId);
/*        Task<bool> UpdateCart(CartDto cartDto);*/
        Task<CartDto> SaveOrUpdateCart(CartDto dto);

        Task<bool> DeleteFromCart(long cartDetailsId);

        Task<bool> ClearCart(String userId);

        Task<bool> ApplyCoupon(String userId, string couponCode);

        Task<bool> RemoveCoupon(String userId);

    }
}
