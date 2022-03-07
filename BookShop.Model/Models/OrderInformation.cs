using System;

namespace BookShop.Model.Models
{
    public class OrderInformation
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Status { get; set; }
        public string PaymentStatus { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string CustomerId { get; set; }

        public string CustomerAddressDistrict { get; set; }
        public string CustomerAddressCity { get; set; }
        public string CustomerAddressWard { get; set; }
        public int Weight { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentStatus { get; set; }
        public string RateId { get; set; }
    }
}
