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
            Cart cart = new()
            {
                CartHeader = await _context.CartHeaders
                    .FirstOrDefaultAsync(c => c.UserId == userId),
            };
            cart.CartDetails = _context.CartDetails
                .Where(c => c.CartHeaderId == cart.CartHeader.Id)
                    .Include(c => c.Product);
            return _mapper.Map<CartDto >(cart);
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
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync(); // Salva o novo produto no banco
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
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();

                // Define a relação entre o detalhe do carrinho e o cabeçalho recém-criado
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;

                // Remove a referência direta ao produto (para evitar erro de referência circular no EF)
                cart.CartDetails.FirstOrDefault().Product = null;

                // Adiciona o item (CartDetail) ao banco de dados
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                // --- ETAPA 4: Se o  cabesalho do carrinho do usuário já existe ---
                // Verifica se o produto que está sendo adicionado já existe dentro desse carrinho
                var cartDetail = await _context.CartDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p =>
                        p.ProductId == dto.CartDetails.FirstOrDefault().ProductId &&
                        p.CartHeaderId == cartHeader.Id);

                // --- ETAPA 4.1: Se o produto ainda não estiver no carrinho ---
                if (cartDetail == null)
                {
                    // Define o ID do cabeçalho existente
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;

                    // Evita regravação do produto inteiro (mantém apenas referência)
                    cart.CartDetails.FirstOrDefault().Product = null;

                    // Adiciona o novo item ao carrinho
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // --- ETAPA 4.2: Se o produto já existir no carrinho ---
                    // Atualiza a quantidade (soma com a anterior)
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;

                    // Mantém o mesmo ID e CartHeaderId (para sobrescrever o item existente)
                    cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;

                    // Atualiza o item existente no banco
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
