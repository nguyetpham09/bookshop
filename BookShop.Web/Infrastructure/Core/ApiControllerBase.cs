using BookShop.Model.Models;
using BookShop.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Http;

namespace BookShop.Web.Infrastructure.Core
{
    public class ApiControllerBase : ApiController
    {
        private readonly IErrorService _errorService;

        public ApiControllerBase()
        {
        }
        public ApiControllerBase(IErrorService errorService)
        {
            _errorService = errorService;
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage requestMessage, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage responseMessage = null;

            try
            {
                responseMessage = function.Invoke();
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                foreach (var eve in dbEntityValidationException.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                LogError(dbEntityValidationException);
                responseMessage = requestMessage.CreateResponse(System.Net.HttpStatusCode.BadRequest, dbEntityValidationException.Message);
            }
            catch (DbUpdateException dbUpdateEx)
            {
                LogError(dbUpdateEx);
                responseMessage = requestMessage.CreateResponse(System.Net.HttpStatusCode.BadRequest, dbUpdateEx.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                responseMessage = requestMessage.CreateResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }

            return responseMessage;
        }

        private void LogError(Exception ex)
        {
            try
            {
                Error error = new Error();
                error.Message = ex.Message;
                error.CreatedDate = DateTime.Now;
                error.StackTrace = ex.StackTrace;
                _errorService.Create(error);
                _errorService.Save();
            }
            catch
            {
            }
        }
    }
}