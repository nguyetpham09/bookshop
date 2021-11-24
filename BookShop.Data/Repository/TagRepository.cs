using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface ITagRepository : IRepository<Tag>
    {

    }

    public class TagRepository : RepositoryBase<Tag>
    {
        public TagRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
