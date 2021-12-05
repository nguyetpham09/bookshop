using AutoMapper;
using BookShop.Service;
using BookShop.Web.Infrastructure.Core;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var productCategory = _productCategoryService.GetAll();

                var productCategoryVm = Mapper.Map<List<ProductCategoryViewModel>>(productCategory);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, productCategoryVm);

                return response;
            });
        }
    }
}