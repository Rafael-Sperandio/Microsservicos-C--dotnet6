using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.CartAPI.Model.Base
{
    //TODO Nome Mudar

    //quantidade de itens de uma linha do carrinho
    [Table("cart_detail")]
    public class CartDetail : BaseEntity
    {
        //pedido do carrinho especifico
        public long CartHeaderId { get; set; }

        [ForeignKey("cart_header_id")]
        public CartHeader CartHeader { get; set; }  


        public long ProductId { get; set; }

        [ForeignKey("product_id")]
        public Product Product { get; set; }


        [Column("count")]
        public long Count { get; set; }


    }
}
