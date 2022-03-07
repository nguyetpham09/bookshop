using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
    }

    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}