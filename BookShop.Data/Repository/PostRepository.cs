using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public class PostRepository : RepositoryBase<Post>
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
