using AutoMapper;
using BookShop.Model.Models;
using BookShop.Service;
using BookShop.Web.Infrastructure.Core;
using BookShop.Web.Infrastructure.Extension;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace BookShop.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ApiControllerBase
    {
        #region Initialize

        private IProductCategoryService _productCategoryService;

        public CategoryController()
        {
        }

        public CategoryController(IErrorService errorService, IProductCategoryService productCategoryService) : base(errorService)
        {
            _productCategoryService = productCategoryService;
        }

        #endregion Initialize

        [HttpGet]
        [Route("getallparents")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll();

                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.Accepted, responseData);
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryViewModel)
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
                    var dbProductCategory = _productCategoryService.GetById(productCategoryViewModel.Id);
                    dbProductCategory.UpdateProductCategory(productCategoryViewModel);

                    _productCategoryService.Update(dbProductCategory);
                    _productCategoryService.Save();

                    var responseDate = Mapper.Map<ProductCategoryViewModel>(dbProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, dbProductCategory);
                }

                return response;
            });
        }

        
    }
}