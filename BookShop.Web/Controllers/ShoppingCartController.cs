using AutoMapper;
using BookShop.Common;
using BookShop.Model.Models;
using BookShop.Service;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TeduShop.Common;
using TeduShop.Web.App_Start;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Infrastructure.GoShipAPI;
using TeduShop.Web.Infrastructure.MomoSecurity;
using TeduShop.Web.Infrastructure.NganLuongAPI;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        IOrderService _orderService;
        private ApplicationUserManager _userManager;

        private string merchantId = ConfigHelper.GetByKey("MerchantId");
        private string merchantPassword = ConfigHelper.GetByKey("MerchantPassword");
        private string merchantEmail = ConfigHelper.GetByKey("MerchantEmail");

        private string endpoint = ConfigHelper.GetByKey("endpoint");
        private string partnerCode = ConfigHelper.GetByKey("partnerCode");
        private string accessKey = ConfigHelper.GetByKey("accessKey");
        private string secretKey = ConfigHelper.GetByKey("secretKey");
        private string redirectUrl = ConfigHelper.GetByKey("redirectUrl");
        private string ipnUrl = ConfigHelper.GetByKey("ipnUrl");

        public ShoppingCartController(IOrderService orderService, IProductService productService, ApplicationUserManager userManager)
        {
            this._productService = productService;
            this._userManager = userManager;
            this._orderService = orderService;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();

            GoShip goship = new GoShip();
            HttpClient client = new HttpClient();
            var result = goship.GetCities(client);
            var sData = result.Content.ReadAsStringAsync().Result;
            dynamic goshipResponse = JsonConvert.DeserializeObject(sData);
            var data = goshipResponse.data;
            string myString = Convert.ToString(data);

            var listCity = JsonConvert.DeserializeObject<IEnumerable<GoShipCity>>(myString);

            ViewBag.ListCity = listCity;
            return View();
        }

        public ActionResult CheckOut()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }
        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public ActionResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
  
            var orderNew = new Order();

            orderNew.UpdateOrder(order);
            
            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = item.Product.Price;
                orderDetails.Add(detail);

                isEnough = _productService.SellProduct(item.ProductId, item.Quantity);
                break;
            }
            if (isEnough)
            {
                var orderReturn = _orderService.Create(ref orderNew, orderDetails);
                _productService.Save();

                if (order.PaymentMethod == "CASH")
                {
                    //create shipment 
                    GoShip goship = new GoShip();
                    HttpClient client = new HttpClient();
                    Shipment shipment = new Shipment();
                    shipment.order_id = orderReturn.ID.ToString();
                    shipment.rate = orderNew.RateId;
                    shipment.address_to = new Shipment.Address();
                    shipment.address_to.district = orderNew.CustomerAddressDistrict;
                    shipment.address_to.city = orderNew.CustomerAddressCity;
                    shipment.address_to.ward = orderNew.CustomerAddressWard;
                    shipment.address_to.street = orderNew.CustomerAddress;
                    shipment.address_to.phone = orderNew.CustomerMobile;
                    shipment.address_to.name = orderNew.CustomerName;

                    shipment.address_from = new Shipment.Address();
                    shipment.address_from.district = ConfigHelper.GetByKey("ShopDistrict");
                    shipment.address_from.city = ConfigHelper.GetByKey("ShopCity");
                    shipment.address_from.ward = ConfigHelper.GetByKey("ShopWard");
                    shipment.address_from.street = ConfigHelper.GetByKey("ShopStreet");
                    shipment.address_from.phone = ConfigHelper.GetByKey("ShopPhone");
                    shipment.address_from.name = ConfigHelper.GetByKey("ShopName");

                    shipment.parcel = new Shipment.Parcel();
                    shipment.parcel.weight = orderNew.Weight;
                    shipment.parcel.cod = orderNew.OrderAmount;

                    HttpResponseMessage result = goship.Shipment(client, shipment);
                    var sData = result.Content.ReadAsStringAsync().Result;
                    dynamic goshipResponse = JsonConvert.DeserializeObject(sData);
                    //var data = goshipResponse.data;//
                    //CreateShipmentReturn responseData = JsonConvert.DeserializeObject<CreateShipmentReturn>(myString);

                    //update shipment id to order 
                    if (goshipResponse.status.Value == "success")
                    {
                        _orderService.UpdateShipmentId(int.Parse(shipment.order_id), goshipResponse.id.Value);
                    }
                    else
                    {
                        return Json(new
                        {
                            status = false,
                            message = "Tạo vận đơn thất bại"
                        });
                    }
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    
                    string orderInfo = "Thanh toán đon hàng tại LovebookShop";


                    string amount = orderNew.Total.ToString();//orderDetails.Sum(x => x.Quantity * x.Price).ToString().Split('.').First();
                    string orderId = orderReturn.ID.ToString();//Guid.NewGuid().ToString();
                    string requestId = Guid.NewGuid().ToString();
                    string extraData = "";
                    string requestType = "captureWallet";

                    string rawHash = "accessKey=" + accessKey +
                                     "&amount=" + amount +
                                     "&extraData=" + extraData +
                                     "&ipnUrl=" + ipnUrl +
                                     "&orderId=" + orderId +
                                     "&orderInfo=" + orderInfo +
                                     "&partnerCode=" + partnerCode +
                                     "&redirectUrl=" + redirectUrl +
                                     "&requestId=" + requestId +
                                     "&requestType=" + requestType;

                    MomoSecurity crypto = new MomoSecurity();
                    string signature = crypto.signSHA256(rawHash, secretKey);

                    JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };

                    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                    JObject jmessage = JObject.Parse(responseFromMomo);
                    var myTest = jmessage.GetValue("payUrl").ToString();
                    System.Diagnostics.Process.Start(jmessage.GetValue("payUrl").ToString());
                    return Redirect(jmessage.GetValue("payUrl").ToString());

                }

            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Không đủ hàng."
                });
            }

        }
        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetById(productId);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if (product.Quantity == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                });
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }
        public ActionResult ReturnUrl()
        {
            string param = "accessKey=" + accessKey +
                 "&amount=" + Request["amount"] +
                 "&extraData=" + Request["extraData"] +
                 "&message=" + Request["message"] +
                 "&orderId=" + Request["orderId"] +
                 "&orderInfo=" + Request["orderInfo"] +
                 "&orderType=" + Request["orderType"] +
                 "&partnerCode=" + Request["partnerCode"] +
                 "&payType=" + Request["payType"] +
                 "&requestId=" + Request["requestId"] +
                 "&responseTime=" + Request["responseTime"] +
                 "&resultCode=" + Request["resultCode"] +
                 "&transId=" + Request["transId"];

            MomoSecurity crypto = new MomoSecurity();
            string signature = crypto.signSHA256(param, secretKey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.message = "Thông tin request không hợp lệ";
                return View();
            }
            if (!Request.QueryString["resultCode"].Equals("0"))
            {
                ViewBag.message = "Thanh toán thất bại!";
            }
            else
            {
                ViewBag.message = "Thanh toán thành công";
                //create SHipment 
                GoShip goship = new GoShip();
                HttpClient client = new HttpClient();
                Shipment shipment = new Shipment();
                shipment.order_id = Request["orderId"];
                OrderInformation orderInfo = _orderService.GetOrderInformationByOrderId(int.Parse(shipment.order_id));
                shipment.rate = orderInfo.RateId;
                shipment.address_to = new Shipment.Address();
                shipment.address_to.district = orderInfo.CustomerAddressDistrict;
                shipment.address_to.city = orderInfo.CustomerAddressCity;
                shipment.address_to.ward = orderInfo.CustomerAddressWard;
                shipment.address_to.street = orderInfo.CustomerAddress;
                shipment.address_to.phone = orderInfo.CustomerPhoneNumber;
                shipment.address_to.name = orderInfo.CustomerName;

                shipment.address_from = new Shipment.Address();
                shipment.address_from.district = ConfigHelper.GetByKey("ShopDistrict");
                shipment.address_from.city = ConfigHelper.GetByKey("ShopCity");
                shipment.address_from.ward = ConfigHelper.GetByKey("ShopWard");
                shipment.address_from.street = ConfigHelper.GetByKey("ShopStreet");
                shipment.address_from.phone = ConfigHelper.GetByKey("ShopPhone");
                shipment.address_from.name = ConfigHelper.GetByKey("ShopName");

                shipment.parcel = new Shipment.Parcel();
                shipment.parcel.weight = orderInfo.Weight;

                HttpResponseMessage result = goship.Shipment(client,shipment);
                var sData = result.Content.ReadAsStringAsync().Result;
                dynamic goshipResponse = JsonConvert.DeserializeObject(sData);
                if (goshipResponse.status.Value == "success")
                {
                    _orderService.UpdateShipmentId(int.Parse(shipment.order_id), goshipResponse.id.Value);
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message ="Tạo vận đơn thất bại"
                    });
                }
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            }

            return View();

        }
        public ActionResult ConfirmOrder()
        {
            string token = Request["token"];
            RequestCheckOrder info = new RequestCheckOrder();
            info.Merchant_id = merchantId;
            info.Merchant_password = merchantPassword;
            info.Token = token;
            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);
            if (result.errorCode == "00")
            {
                //update status order
                _orderService.UpdateStatus(int.Parse(result.order_code));
                _orderService.Save();
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";
            }
            else
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Có lỗi xảy ra. Vui lòng liên hệ admin.";
            }
            return View();
        }
        public ActionResult CancelOrder()
        {
            return View();
        }
    }
}