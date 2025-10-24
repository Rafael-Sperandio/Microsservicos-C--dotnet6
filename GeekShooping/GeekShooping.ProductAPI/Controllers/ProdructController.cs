using GeekShooping.ProductAPI.Data.Dto;
using GeekShooping.ProductAPI.Model;
using GeekShooping.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdructController : ControllerBase
    {
        private readonly ILogger<ProdructController> _logger;
        private readonly IProdructRepository _repository;
        public ProdructController(ILogger<ProdructController> logger, IProdructRepository repository)
        {
            _logger = logger;
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductDto>> Get(long id)
        {
            var product= await _repository.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _repository.GetAll();
            return Ok(products);
        }

        [HttpPost("")]
        public async Task<ActionResult<ProductDto>> Create(ProductDto dto)
        {
            if (dto == null) return BadRequest("Produto nulo");
            var product = await _repository.Create(dto);
            return Ok(product);
        }
        [HttpPut("")]
        public async Task<ActionResult<ProductDto>> Update(ProductDto dto)
        {
            if (dto == null) return BadRequest("Produto nulo");
            var product = await _repository.Upddate(dto);
            return Ok(product);
        }
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ProductDto>> Delete(long id)
        {
            var deletado = await _repository.Delete(id);
            if (!deletado) return BadRequest();
            return Ok(deletado);
        }
    }
}
