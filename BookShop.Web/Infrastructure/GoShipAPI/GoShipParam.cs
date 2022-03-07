namespace BookShop.Web.Infrastructure.GoShipAPI
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