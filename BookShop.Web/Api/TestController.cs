

using AutoMapper;
using BookShop.Service;
using BookShop.Web.Infrastructure.Common;
using BookShop.Web.Infrastructure.Core;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Results;

namespace BookShop.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ApiControllerBase
    {
        private readonly IPostCategoryService _postCategoryService;
        public TestController()
        { }
        public TestController(IErrorService errorService, IPostCategoryService postCategoryService) : base(errorService)
        {
            _postCategoryService = postCategoryService;
        }
        /*[Route("getall")]
        [HttpGet]
        public IActionResult Get()
        {
            /*return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    
                }

                return response;
            })
            //return (IActionResult)Ok();

            if (ModelState.IsValid)
            {
                //request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ModelState);
                //return Ok(ApiResponse<string>.Fail("400", "Bad request"));
                return null;
            }
            else
            {
                var listCategory = _postCategoryService.GetAll();

                var listPostCategoryVm = Mapper.Map<List<PostCategoryViewModel>>(listCategory);

                //return Ok(ApiResponse<List<PostCategoryViewModel>>.Success(listPostCategoryVm));
                return null;
            }

          

        }*/

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    response = request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listCategory = _postCategoryService.GetAll();

                    var listPostCategoryVm = Mapper.Map<List<PostCategoryViewModel>>(listCategory);

                    response = request.CreateResponse(System.Net.HttpStatusCode.OK, listPostCategoryVm);
                }

                return response;
            });
        }
    }
}
