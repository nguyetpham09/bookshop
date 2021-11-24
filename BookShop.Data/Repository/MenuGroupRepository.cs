using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface IMenuGroupRepository : IRepository<MenuGroup>
    {

    }

    public class MenuGroupRepository : RepositoryBase<MenuGroup>
    {
        public MenuGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
