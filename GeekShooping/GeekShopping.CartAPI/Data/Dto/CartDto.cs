namespace GeekShooping.CartAPI.Data.Dto
{

    public class CartDto
    {
        public long Id { get; set; }
        public CartHeaderDto CartHeader { get; set; }


        //relacionamentos produto_cliente
        public IEnumerable<CartDetailDto> CartDetails { get; set; }

    }
}
