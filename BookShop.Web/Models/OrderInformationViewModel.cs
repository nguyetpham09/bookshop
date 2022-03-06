using System;

namespace TeduShop.Web.Models
{
    [Serializable]
    public class OrderInformationViewModel
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
    }
}