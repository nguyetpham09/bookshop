using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {

    }

    public class OrderDetailRepository : RepositoryBase<OrderDetail>
    {
        public OrderDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
