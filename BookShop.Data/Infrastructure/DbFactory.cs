namespace TeduShop.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private BookShopDbContext dbContext;

        public BookShopDbContext Init()
        {
            return dbContext ?? (dbContext = new BookShopDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null) dbContext.Dispose();
        }
    }
}
