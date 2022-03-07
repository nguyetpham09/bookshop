using AutoMapper;
using BookShop.Model.Models;
using BookShop.Service;
using System.Collections.Generic;
using System.Web.Mvc;
using TeduShop.Web.Models;

namespace BookShop.Web.Controllers
{
    public class OrderController : Controller
    {
        IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: OrderHistory 
        [HttpGet]
        public ActionResult Index(string id)
        {
            var orderInformationModel = _orderService.GetOrdersInformationByUserId(id);
            var viewModel = Mapper.Map<IEnumerable<OrderInformation>, IEnumerable<OrderInformationViewModel>>(orderInformationModel);
            return View(viewModel);
        }
    }
}