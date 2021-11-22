using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public class PostCategoryRepository : RepositoryBase<PostCategory>
    {
        public PostCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
