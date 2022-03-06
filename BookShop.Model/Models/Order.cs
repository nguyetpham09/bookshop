using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeduShop.Model.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerEmail { set; get; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { set; get; }

        [MaxLength(256)]
        public string PaymentMethod { set; get; }

        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public string PaymentStatus { set; get; }
        public bool Status { set; get; }

        public string CustomerAddressDistrict { get; set; }
        public string CustomerAddressCity { get; set; }
        public string CustomerAddressWard { get; set; }
        public int Weight { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentStatus { get; set; }
        public string RateId { get; set; }
        public int OrderAmount { get; set; }
        public int CodFee { get; set; }
        public int Total { get; set; }
        [StringLength(128)]
        [Column(TypeName = "nvarchar")]
        public string CustomerId { set; get; }

        [ForeignKey("CustomerId")]
        public virtual ApplicationUser User { set; get; }

        public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }
    }
}