namespace BookShop.Web.Models
{
    public class PostTagViewModel
    {
        public int PostId { get; set; }

        public string TagId { get; set; }

        public virtual PostViewModel Post { get; set; }

        public virtual TagViewModel Tag { get; set; }
    }
}