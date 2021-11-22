using BookShop.Data.Infrastructure;
using BookShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.Data.Repository
{
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ProductCategory> GetProductCategoriesByAlias(string alias)
        {
            return DbContext.ProductCategories.Where(x => x.Alias == alias);
        }
    }
}
