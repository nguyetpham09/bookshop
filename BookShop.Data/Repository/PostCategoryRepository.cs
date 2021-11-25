using BookShop.Data.Infrastructure;
using BookShop.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BookShop.Data.Repository
{
    public interface IPostCategoryRepository : IRepository<PostCategory> 
    {
        
    }

    public class PostCategoryRepository : RepositoryBase<PostCategory>, IPostCategoryRepository
    {
        public PostCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
