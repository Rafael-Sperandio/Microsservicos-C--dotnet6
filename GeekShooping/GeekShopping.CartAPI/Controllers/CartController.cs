using GeekShooping.CartAPI.Data.Dto;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
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
        private IRabbitMQMessageSender _rabbitMQMessageSender;


        public CartController(ILogger<CartController> logger, ICartRepository repository, IRabbitMQMessageSender rabbitMQMessageSender)
        {

            _logger = logger;
            _repository = repository ?? throw new
                ArgumentNullException(nameof(repository));
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new
                ArgumentNullException(nameof(rabbitMQMessageSender));
        }

        [HttpGet("get-cart/{userId}")]
        public async Task<ActionResult<CartDto>> GetById(string userId)
        {
            var cart = await _repository.GetCartByUserId(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartDto>> AddCart(CartDto model)
        {
            var cart = await _repository.SaveOrUpdateCart(model);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartDto>> ApplyCoupon(CartDto dto)
        {
            var cart = await _repository.ApplyCoupon(dto.CartHeader.UserId,dto.CartHeader.CouponCode);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("remove-coupon/{userId}")]
        public async Task<ActionResult<CartDto>> RemoveCoupon(string userId)
        {
            var cart = await _repository.RemoveCoupon(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("update-cart")]
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

        //
        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderDto>> Checkout(CheckoutHeaderDto dto)
        {
            if(dto?.UserId== null) return BadRequest();

            var cart = await _repository.GetCartByUserId(dto.UserId);
            if (cart == null) return NotFound();
            dto.CartDetails = cart.CartDetails;
            dto.DateTime = DateTime.Now;

            //RabbitMQ logic comes here!!!
            _rabbitMQMessageSender.SendMessage(dto, "checkoutqueue");


            return Ok(dto);
        }
    }
}
