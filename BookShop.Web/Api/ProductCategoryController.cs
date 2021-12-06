using AutoMapper;
using BookShop.Service;
using BookShop.Web.Infrastructure.Core;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace BookShop.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : ApiControllerBase
    {
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController()
        {
        }

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService) : base(errorService)
        {
            _productCategoryService = productCategoryService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, int page, int pageSize)
        {
            return CreateHttpResponse(request, () =>
            {
                var totalRow = 0;

                var productCategory = _productCategoryService.GetAll();

                totalRow = productCategory.Count();

                var query = productCategory.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var productCategoryVm = Mapper.Map<List<ProductCategoryViewModel>>(query);

                var pagination = new PaginationSet<ProductCategoryViewModel>()
                {
                    Items = productCategoryVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = totalRow / pageSize
                };

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, pagination);

                return response;
            });
        }
    }
}