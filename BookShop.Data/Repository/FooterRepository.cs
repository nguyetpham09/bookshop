using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface IFooterRepository : IRepository<Footer>
    {

    }

    public class FooterRepository : RepositoryBase<Footer>
    {
        public FooterRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
