using AutoMapper;
using GeekShooping.CartAPI.Data.Dto;
using GeekShooping.CartAPI.Model.Base;
using GeekShopping.CartAPI.Model.Context;
using GeekShopping.CartAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository
{
    public class CartRepository : ICartRepository
    {

        private readonly MySQLContext _context;
        private IMapper _mapper;
        public CartRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCoupon(string userId, long couponCode)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeader = await _context.CartHeaders
                        .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartHeader != null)
            {
                _context.CartDetails
                    .RemoveRange(
                    _context.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id));
                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> DeleteFromCart(long cartDetailsId)
        {
            try
            {

                CartDetail cartDetail = await _context.CartDetails
                    .FirstOrDefaultAsync(c => c.Id == cartDetailsId);

                int total = _context.CartDetails
                    .Where(c => c.CartHeaderId == cartDetail.CartHeaderId).Count();

                _context.CartDetails.Remove(cartDetail);

                if (total == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders
                        .FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            var header = await _context.CartHeaders
                            .FirstOrDefaultAsync(c => c.UserId == userId);

            if (header == null)
                return new CartDto(); // retorna carrinho vazio (ou null, se preferir)
            Cart cart = new()
            {
                CartHeader = header,
                CartDetails = await _context.CartDetails
                    .Where(c => c.CartHeaderId == header.Id)
                    .Include(c => c.Product)
                    .ToListAsync()
            };
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            throw new NotImplementedException();
        }

        /*        public async Task<bool> UpdateCart(CartDto cartDto)
                {
                    throw new NotImplementedException();
                }*/

        //metodo esta fazendo save e Update o que acaba gerando confusão
        //TODO correto seriar serparar em duas partes save e Update
        public async Task<CartDto> SaveOrUpdateCart(CartDto dto)
        {
            Cart cart = _mapper.Map<Cart>(dto);
            // --- ETAPA 1: Verifica se o produto já existe no banco de dados ---
            // Busca o produto com o mesmo ID do primeiro item do carrinho recebido
            var product = await _context.Products.FirstOrDefaultAsync(
                p => p.Id == dto.CartDetails.FirstOrDefault().ProductId);

            // Se o produto não existir no banco, ele é adicionado
            if (product == null)
            {
                var newProduct = cart.CartDetails.FirstOrDefault().Product;
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();

                // Atualiza o ID do produto dentro do CartDetail
                cart.CartDetails.FirstOrDefault().ProductId = newProduct.Id;
            }
            else
            {
                // Se o produto já existia, usa o ID real do banco
                cart.CartDetails.FirstOrDefault().ProductId = product.Id;
            }

            // --- ETAPA 2: Verifica se o usuário já possui um CartHeader (cabeçalho do carrinho) ---
            // O CartHeader identifica o carrinho do usuário (ex: UserId)
            var cartHeader = await _context.CartHeaders
                .AsNoTracking() // Evita rastreamento para melhorar performance
                .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

            // --- ETAPA 3: Se o usuário não tiver carrinho, cria um novo ---
            if (cartHeader == null)
            {
                // Cria o cabeçalho do carrinho (CartHeader)
                var newHeader = cart.CartHeader;
                _context.CartHeaders.Add(newHeader);
                await _context.SaveChangesAsync();

                // Usa o ID real do banco
                var headerId = newHeader.Id;

                // Define o relacionamento corretamente
                var newDetail = cart.CartDetails.FirstOrDefault();
                newDetail.CartHeaderId = headerId;
                newDetail.CartHeader = null;
                newDetail.Product = null;

                // Agora sim adiciona o detalhe
                _context.CartDetails.Add(newDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Verifica se o produto já existe no carrinho
                var cartDetail = await _context.CartDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p =>
                        p.ProductId == dto.CartDetails.FirstOrDefault().ProductId &&
                        p.CartHeaderId == cartHeader.Id);

                if (cartDetail == null)
                {
                    // Atualiza referência de CartHeaderId corretamente
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;

                    // Garante que o EF não tente criar um novo CartHeader
                    cart.CartHeader = null;

                    // Evita tentativa de inserir novamente o produto
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().CartHeader = null;

                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Atualiza quantidade se já existir
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;

                    cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;

                    // Evita tentativa de inserir novamente o produto
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().CartHeader = null;

                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }

            // --- ETAPA 5: Retorna o carrinho atualizado ---
            // Converte novamente para DTO para ser enviado de volta à camada superior (ex: API)
            return _mapper.Map<CartDto>(cart);
        }
    }
}
