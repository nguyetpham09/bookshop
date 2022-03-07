using BookShop.Data.Infrastructure;
using BookShop.Data.Repositories;
using BookShop.Model.Models;

namespace BookShop.Service
{
    public interface IPageService
    {
        Page GetByAlias(string alias);
    }
    public class PageService : IPageService
    {
        IPageRepository _pageRepository;
        IUnitOfWork _unitOfWork;
        public PageService(IPageRepository pageRepository, IUnitOfWork unitOfWork)
        {
            this._pageRepository = pageRepository;
            this._unitOfWork = unitOfWork;
        }
        public Page GetByAlias(string alias)
        {
            return _pageRepository.GetSingleByCondition(x => x.Alias == alias);
        }
    }
}
