using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface IMenuRepository : IRepository<Menu>
    {

    }

    public class MenuRepository : RepositoryBase<Menu>
    {
        public MenuRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
