using BookShop.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BookShop.Web.Infrastructure.GoShipAPI
{
    public class GoShip
    {
        //private string ClientId, ClientSecret, UserName, Password, AccessToken, BaseUrl, RefreshToken;

        private const string LOGIN_PATH = "login";
        private const string GET_CITIES_PATH = "cities";
        private const string GET_DISTRICTS_PATH = "cities/{code}/districts";
        private const string GET_WARD_PATH = "districts/{code}/wards";
        private const string RATE_PATH = "rates";
        private const string SHIPMENT_PATH = "shipments";

        private string UserName = ConfigHelper.GetByKey("UserName");
        private string Password = ConfigHelper.GetByKey("Password");
        private string ClientId = ConfigHelper.GetByKey("ClientId");
        private string ClientSecret = ConfigHelper.GetByKey("ClientSecret");
        private string AccessToken = ConfigHelper.GetByKey("AccessToken");
        private string BaseUrl = ConfigHelper.GetByKey("BaseUrl");
        private string RefreshToken = ConfigHelper.GetByKey("RefreshToken");

        private bool Refresh(HttpClient client)
        {
            string path = "refresh_token";
            GoShipParam param = new GoShipParam();
            param.RefreshToken = RefreshToken;
            param.ClientId = ClientId;
            param.ClientSecret = ClientSecret;

            //request
            HttpResponseMessage response = Request(client, HttpMethod.Post, JsonConvert.SerializeObject(param), path);
            // refresh error
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                //login
                return Login(client);
            }
            //refresh success
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //read data
                string sData = response.Content.ReadAsStringAsync().Result;
                dynamic goshipResponse = JsonConvert.DeserializeObject(sData);

                //save access token
                AccessToken = goshipResponse.access_token;
                ConfigHelper.SetByKey("AccessToken", goshipResponse.access_token);

                return true;
            }

            return false;
        }

        private bool Login(HttpClient client)
        {
            GoShipParam param = new GoShipParam();
            param.UserName = UserName;
            param.ClientId = ClientId;
            param.Password = Password;
            param.ClientSecret = ClientSecret;

            string url = this.BaseUrl + LOGIN_PATH;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            //set access token to header
            request.Headers.Add("Accept", "application/json");
            //send request
            if (param != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = client.SendAsync(request).Result;
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //read data
                string sData = response.Content.ReadAsStringAsync().Result;
                dynamic goshipResponse = JsonConvert.DeserializeObject(sData);
                // save access token
                AccessToken = goshipResponse.access_token;
                ConfigHelper.SetByKey("AccessToken", goshipResponse.access_token);
                return true;
            }
            return false;
        }

        public HttpResponseMessage Request (HttpClient client, HttpMethod method, Object param, string path)
        {
            string url = this.BaseUrl + path;
            HttpRequestMessage request = new HttpRequestMessage(method, url);

            //set access token to header
            request.Headers.Add("Authorization", "Bearer " + AccessToken);
            request.Headers.Add("Accept", "application/json");
            //send request
            if (param != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = client.SendAsync(request).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //read data
                string sData = response.Content.ReadAsStringAsync().Result;
                dynamic goshipResponse = JsonConvert.DeserializeObject(sData);
                // save access token
                var data = goshipResponse.data;
            }

            // login and give it another try
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (Refresh(client))
                {
                    response = Request(client, method, param, path);
                    return response;
                }
            }
            return response;
        }

        public HttpResponseMessage GetCities (HttpClient client)
        {
            return Request(client, HttpMethod.Get, null, GET_CITIES_PATH);
        }

        public HttpResponseMessage GetDistricts(HttpClient client, string cityCode)
        {
            return Request(client, HttpMethod.Get, null, GET_DISTRICTS_PATH.Replace("{code}", cityCode));
        }

        public HttpResponseMessage GetWards(HttpClient client, string districtCode)
        {
            return Request(client, HttpMethod.Get, null, GET_WARD_PATH.Replace("{code}", districtCode));
        }
        public HttpResponseMessage GetRate(HttpClient client, Shipment data)
        {
            Dictionary<string, object> obj = new Dictionary<string, object> ();
            obj.Add("shipment", data);
            return Request(client, HttpMethod.Post, obj, RATE_PATH);
        }
        public HttpResponseMessage Shipment(HttpClient client, Shipment data)
        {
            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("shipment", data);
            return Request(client, HttpMethod.Post, obj, SHIPMENT_PATH);
        }

    }


}