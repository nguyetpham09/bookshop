namespace BookShop.Web.Models
{
    public class ProductTagViewModel
    {
        public int ProductId { get; set; }

        public int TagId { get; set; }

        public virtual ProductViewModel Product { get; set; }

        public virtual TagViewModel Tag { get; set; }
    }
}