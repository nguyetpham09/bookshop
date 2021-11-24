using BookShop.Data.Infrastructure;
using BookShop.Model.Models;

namespace BookShop.Data.Repository
{
    public interface IVisitorStatisticRepository : IRepository<VisitorStatistic>
    {

    }

    public class VisitorStatisticRepository : RepositoryBase<VisitorStatistic>
    {
        public VisitorStatisticRepository(IDbFactory dbFactory) : base(dbFactory)
        {
                
        }
    }
}
