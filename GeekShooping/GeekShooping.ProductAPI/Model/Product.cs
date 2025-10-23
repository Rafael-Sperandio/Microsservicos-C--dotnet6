using GeekShooping.ProductAPI.Model.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShooping.ProductAPI.Model
{
    [Table("product")]
    public class Product : BaseEntity
    {
        [Column("name")]
        [Required]
        [StringLength(150)]
        public string Name {  get; set; }

        [Column("price")]
        [Range(0,1000000)]
        public string Price { get; set; }

        [Column("category_name")]
        [StringLength(70)]
        public string CategoryName { get; set; }

        [Column("image_url")]
        [StringLength(260)]
        public string ImageUrl { get; set; }

    }
}
