using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface IPageRepository : IRepository<Page>
    {

    }

    public class PageRepository : RepositoryBase<Page>
    {
        public PageRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
