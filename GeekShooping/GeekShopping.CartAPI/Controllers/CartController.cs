using GeekShooping.CartAPI.Data.Dto;
using GeekShopping.CartAPI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {

        private readonly ILogger<CartController> _logger;
        private ICartRepository _repository;


        public CartController(ILogger<CartController> logger, ICartRepository repository)
        {
            _logger = logger;
            _repository = repository ?? throw new
                ArgumentNullException(nameof(repository));
        }

        [HttpGet("get-cart/{id}")]
        public async Task<ActionResult<CartDto>> GetById(string userId)
        {
            var cart = await _repository.GetCartByUserId(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart/{id}")]
        public async Task<ActionResult<CartDto>> AddCart(CartDto vo)
        {
            var cart = await _repository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("update-cart/{id}")]
        public async Task<ActionResult<CartDto>> UpdateCart(CartDto vo)
        {
            var cart = await _repository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartDto>> DeleteCart(int id)
        {
            var status = await _repository.DeleteFromCart(id);
            if (!status) return BadRequest();
            return Ok(status);
        }
    }
}
