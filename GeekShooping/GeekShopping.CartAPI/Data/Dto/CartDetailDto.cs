using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.CartAPI.Data.Dto
{
    public class CartDetailDto 
    {
        public long Id { get; set; }

        public long CartHeaderId { get; set; }

        public CartHeaderDto CartHeader { get; set; }  

        public long ProductId { get; set; }


        public ProductDto ProductDto { get; set; }

        public long Count { get; set; }


    }
}
