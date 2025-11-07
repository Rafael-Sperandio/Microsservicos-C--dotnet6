using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.CartAPI.Model.Base
{
    //[Table("cart")]
    public class Cart : BaseEntity
    {

        public CartHeader CartHeader { get; set; }

        //relacionamentos produto_cliente
        public IEnumerable<CartDetail> CartDetails { get; set; }

    }
}
