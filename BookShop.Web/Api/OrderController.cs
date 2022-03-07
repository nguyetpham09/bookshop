using AutoMapper;
using BookShop.Model.Models;
using BookShop.Service;
using BookShop.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeduShop.Web.Models;

namespace BookShop.Web.Api
{
    [RoutePrefix("api/order")]
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        #region Initialize
        private IOrderService _orderService;

        public OrderController(IErrorService errorService, IOrderService orderService)
            : base(errorService)
        {
            this._orderService = orderService;
        }

        #endregion
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize, int? orderId)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderService.GetAllOrderInformation();

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<OrderInformation>, IEnumerable<OrderInformationViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<OrderInformationViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }
    }
}
