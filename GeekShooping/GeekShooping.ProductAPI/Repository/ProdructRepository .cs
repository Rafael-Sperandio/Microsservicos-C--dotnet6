using AutoMapper;
using GeekShooping.ProductAPI.Data.Dto;
using GeekShooping.ProductAPI.Model;
using GeekShooping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShooping.ProductAPI.Repository
{
    public class  ProdructRepository : IProdructRepository
    {
        private readonly MySQLContext _contex;
        private IMapper _mapper;
        public ProdructRepository(MySQLContext contex, IMapper mapper)
        {
            _contex = contex;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            List<Product> products = await _contex.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetById(long id)
        {
            Product product = await _contex.Products.Where(p => p.id == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> Create(ProductDto dto)
        {
            Product product = _mapper.Map<Product>(dto);
             _contex.Products.Add(product);
            await _contex.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);

        }


        public async Task<ProductDto> Upddate(ProductDto dto)
        {
            Product product = _mapper.Map<Product>(dto);
            _contex.Products.Update(product);
            await _contex.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Product product = await _contex.Products.Where(p => p.id == id).FirstOrDefaultAsync();
                if (product ==null) return false;
                _contex.Remove(product);
                await _contex.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
