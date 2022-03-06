using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class OrderViewModel
    {
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

        [MaxLength(128)]
        public string CustomerId { set; get; }

        public string BankCode { set; get; }

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

        public IEnumerable<OrderDetailViewModel> OrderDetails { set; get; }
    }
}