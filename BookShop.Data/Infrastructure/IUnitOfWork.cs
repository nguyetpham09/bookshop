namespace BookShop.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
