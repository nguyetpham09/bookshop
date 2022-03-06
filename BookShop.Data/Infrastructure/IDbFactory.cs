using System;

namespace TeduShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BookShopDbContext Init();
    }
}
