using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface IPostTagRepository : IRepository<PostTag>
    {

    }

    public class PostTagRepository : RepositoryBase<PostTag>
    {
        public PostTagRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
