using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.CartAPI.Model.Base
{
    //TODO Nome Mudar

    //quantidade de itens de uma linha do carrinho
    [Table("cart_detail")]
    public class CartDetail : BaseEntity
    {
        //ajuste necesario para vincular a id a entidade
        [Column("cart_header_id")]
        public long CartHeaderId { get; set; }

        //ajuste necesario para vincular a id a entidade
        [ForeignKey(nameof(CartHeaderId))]
        public CartHeader CartHeader { get; set; }

        [Column("product_id")]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [Column("count")]
        public long Count { get; set; }
    }
}
