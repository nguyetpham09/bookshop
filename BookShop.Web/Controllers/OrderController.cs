using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
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