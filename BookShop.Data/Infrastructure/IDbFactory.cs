using System;

namespace BookShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BookShopDbContext Init();
    }
}
