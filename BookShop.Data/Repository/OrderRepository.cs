using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {

    }

    public class OrderRepository : RepositoryBase<Order>
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
