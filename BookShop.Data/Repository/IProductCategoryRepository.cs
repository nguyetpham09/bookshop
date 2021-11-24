using BookShop.Data.Infrastructure;
using BookShop.Model.Models;
using System.Collections.Generic;

namespace BookShop.Data.Repository
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        IEnumerable<ProductCategory> GetProductCategoriesByAlias(string alias);
    }
}
