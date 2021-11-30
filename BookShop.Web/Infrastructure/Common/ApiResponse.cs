using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShop.Web.Infrastructure.Common
{
    public class ApiResponse<T>
    {
        public static ApiResponseFailed<T> Fail(string error_code, string message)
        {
            return new ApiResponseFailed<T> { status = "FAILED", error_code = error_code, message = message };
        }

        public static ApiResponseFailed<T> Fail(string message)
        {
            return new ApiResponseFailed<T> { status = "FAILED", message = message };
        }

        public static ApiResponseSucess<T> Success(T data)
        {
            return new ApiResponseSucess<T> { status = "SUCCESS", data = data };
        }

        public static ApiResponseSucess<T> Success()
        {
            return new ApiResponseSucess<T> { status = "SUCCESS" };
        }
    }

    public class ApiResponseSucess<T>
    {
        public string status { get; set; }
        public T data { get; set; }
    }

    public class ApiResponseFailed<T>
    {
        public string error_code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }
}