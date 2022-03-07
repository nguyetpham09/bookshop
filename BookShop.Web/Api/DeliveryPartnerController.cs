using BookShop.Common;
using BookShop.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.GoShipAPI;

namespace TeduShop.Web.Api
{

    [RoutePrefix("api/delivery_partner")]
   // [Authorize]
    public class DeliveryPartnerController : ApiControllerBase
    {
        #region Initialize

        public DeliveryPartnerController(IErrorService errorService)
            : base(errorService)
        {
        }
        #endregion

        [Route("districts/{code}")]
        [HttpGet]
        //[Authorize(Roles = "ViewUser")]
        public HttpResponseMessage Districts(HttpRequestMessage request, string code)
        {
            if (string.IsNullOrEmpty(code))
            {

                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(code) + " không có giá trị.");
            }

            GoShip goship = new GoShip();
            HttpClient client = new HttpClient();
            var result = goship.GetDistricts(client, code);
            var sData = result.Content.ReadAsStringAsync().Result;
            dynamic goshipResponse = JsonConvert.DeserializeObject(sData);
            var data = goshipResponse.data;
            string myString = Convert.ToString(data);
            var listDistrict = JsonConvert.DeserializeObject<IEnumerable<GoShipDist>>(myString);
            return request.CreateResponse(HttpStatusCode.OK, listDistrict);

        }
        [Route("wards/{code}")]
        [HttpGet]
        //[Authorize(Roles = "ViewUser")]
        public HttpResponseMessage Wards(HttpRequestMessage request, string code)
        {
            if (string.IsNullOrEmpty(code))
            {

                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(code) + " không có giá trị.");
            }

            GoShip goship = new GoShip();
            HttpClient client = new HttpClient();
            var result = goship.GetWards(client, code);
            var sData = result.Content.ReadAsStringAsync().Result;
            dynamic goshipResponse = JsonConvert.DeserializeObject(sData);
            var data = goshipResponse.data;
            string myString = Convert.ToString(data);
            var listDistrict = JsonConvert.DeserializeObject<IEnumerable<GoShipWard>>(myString);
            return request.CreateResponse(HttpStatusCode.OK, listDistrict);

        }

        [Route("rates")]
        [HttpPost]
        //[Authorize(Roles = "ViewUser")]
        public HttpResponseMessage Rate(HttpRequestMessage request,[FromBody] Shipment shipment)
        {
            if (shipment == null)
            {

                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(shipment) + " không có giá trị.");
            }
            shipment.address_from = new Shipment.Address();
            shipment.address_from.district = ConfigHelper.GetByKey("ShopDistrict");
            shipment.address_from.city = ConfigHelper.GetByKey("ShopCity");
            shipment.address_from.ward = ConfigHelper.GetByKey("ShopWard");

            GoShip goship = new GoShip();
            HttpClient client = new HttpClient();
            var result = goship.GetRate(client, shipment);
            var sData = result.Content.ReadAsStringAsync().Result;
            dynamic goshipResponse = JsonConvert.DeserializeObject(sData);
            var data = goshipResponse.data;
            string myString = Convert.ToString(data);
            var listDistrict = JsonConvert.DeserializeObject<IEnumerable<ShipmentData>>(myString);
            return request.CreateResponse(HttpStatusCode.OK, listDistrict);

        }
    }
}