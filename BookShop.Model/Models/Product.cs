using BookShop.Model.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BookShop.Model.Models
{
    [Table("Products")]
    public class Product : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Alias { get; set; }

        public int CategoryId { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public XElement MoreImages { get; set; }

        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int? Warranty { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string Content { get; set; }

        public bool? HomeFlag { get; set; }

        public bool? HotFlag { get; set; }

        public int? ViewCount { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
