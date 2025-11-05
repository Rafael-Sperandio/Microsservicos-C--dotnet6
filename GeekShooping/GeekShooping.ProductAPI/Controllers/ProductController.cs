using GeekShooping.ProductAPI.Data.Dto;
using GeekShooping.ProductAPI.Model;
using GeekShooping.ProductAPI.Repository;
using GeekShoping.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProdructRepository _repository;
        public ProductController(ILogger<ProductController> logger, IProdructRepository repository)
        {
            _logger = logger;
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        [HttpGet("{id:long}")]
        [Authorize]
        public async Task<ActionResult<ProductDto>> Get(long id)
        {
            var product= await _repository.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("")]
        //não precisa de authorização
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _repository.GetAll();
            return Ok(products);
        }

        [HttpPost("")]
        [Authorize]

        public async Task<ActionResult<ProductDto>> Create(ProductDto dto)
        {
            if (dto == null) return BadRequest("Produto nulo");
            var product = await _repository.Create(dto);
            return Ok(product);
        }

        [HttpPut("")]
        [Authorize]

        public async Task<ActionResult<ProductDto>> Update(ProductDto dto)
        {
            if (dto == null) return BadRequest("Produto nulo");
            var product = await _repository.Upddate(dto);
            return Ok(product);
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<ProductDto>> Delete(long id)
        {
            var deletado = await _repository.Delete(id);
            if (!deletado) return BadRequest();
            return Ok(deletado);
        }
    }
}
