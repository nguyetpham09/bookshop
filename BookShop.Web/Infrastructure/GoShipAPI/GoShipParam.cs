using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Infrastructure.GoShipAPI
{
    public class GoShipParam
    {
        public string UserName { get; set; }
        public string ClientId { get; set; }
        public string Password { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }

        public GoShipParam()
        {

        }
    }
}