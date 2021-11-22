using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public class PostTagRepository : RepositoryBase<PostTag>
    {
        public PostTagRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
