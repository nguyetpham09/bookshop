using BookShop.Model.Models;
using System.Collections.Generic;

namespace BookShop.Data.Repository
{
    public interface IProductCategoryRepository
    {
        IEnumerable<ProductCategory> GetProductCategoriesByAlias(string alias);
    }
}
