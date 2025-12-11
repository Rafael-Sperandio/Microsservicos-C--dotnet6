using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.OrderAPI.Model.Base
{
    [Table("order_detail")]
    public class OrderDetail : BaseEntity
    {
        public long OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader OrderHeader { get; set; }

        //não é fk de produto mesmo se estive-se no mesmo banco 
        [Column("ProductId")]
        public long ProductId { get; set; }

        [Column("count")]
        public int Count { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("price")]
        public decimal Price { get; set; }



    }
}

