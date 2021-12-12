using AutoMapper;
using BookShop.Model.Models;
using BookShop.Service;
using BookShop.Web.Infrastructure.Core;
using BookShop.Web.Infrastructure.Extension;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        #region Initialize
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController()
        {
        }

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService) : base(errorService)
        {
            _productCategoryService = productCategoryService;
        }
        #endregion
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetById(id);

                var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [HttpGet]
        [Route("getallparents")]
        public HttpResponseMessage GetAllParents(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll();

                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.Accepted, responseData);
                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request,string keyword, int page, int pageSize)
        {
            return CreateHttpResponse(request, () =>
            {
                var totalRow = 0;

                var productCategory = _productCategoryService.GetAll(keyword);

                totalRow = productCategory.Count();

                var query = productCategory.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var productCategoryVm = Mapper.Map<List<ProductCategoryViewModel>>(query);

                var pagination = new PaginationSet<ProductCategoryViewModel>()
                {
                    Items = productCategoryVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, pagination);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create (HttpRequestMessage request, ProductCategoryViewModel productCategoryViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }

                else
                {
                    var productCategory = new ProductCategory();
                    productCategory.UpdateProductCategory(productCategoryViewModel);

                    _productCategoryService.Add(productCategory);
                    _productCategoryService.Save();

                    var responseDate = Mapper.Map<ProductCategoryViewModel>(productCategory);
                    response = request.CreateResponse(HttpStatusCode.OK, productCategory);
                }

                return response;
            });
        }
    }
}