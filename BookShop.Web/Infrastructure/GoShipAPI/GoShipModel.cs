namespace BookShop.Web.Infrastructure.GoShipAPI
{
    public class GoShipCity
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class GoShipDist
    {
        public string id { get; set; }
        public string name { get; set; }
        public string district_id { get; set; }
    }
    public class GoShipWard
    {
        public string id { get; set; }
        public string name { get; set; }
        public string ward_id { get; set; }
    }

    public class Shipment
    {
        public string rate { get; set; }
        public string order_id { get; set; }
        public Address address_from { get; set; }
        public Address address_to { get; set; }
        public Parcel parcel { get; set; }

        public class Address
        {
            public string district { get; set; }
            public string city { get; set; }
            public string ward { get; set; }
            public string street { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
        }
        public class Parcel
        {
            public int cod { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int length { get; set; }
            public int weight { get; set; }
        }
    }
    public class ShipmentData
    {
        // "id": "OF8xXzc0OQ==",
        public string id { get; set; }
        //"carrier_name": "Vietnam Post",
        public string carrier_name { get; set; }
        //"carrier_logo": "http:\/\/sandbox.goship.io\/storage\/images\/carriers\/vnpost_c.png",
        public string carrier_logo { get; set; }
        //"service": "Tiết kiệm",
        public string service { get; set; }
        //"expected": "Dự kiến giao trong 6 ngày", 
        public string expected { get; set; }
        //"cod_fee": 0,
        public int cod_fee { get; set; }
        //"total_fee": 13000,
        public int total_fee { get; set; }
        //"total_amount": 13000,
        public int total_amount { get; set; }
        //"expected_txt": ""
        public string expected_txt { get; set; }
    }
    public class CreateShipmentReturn {
        public string id { get; set; }
        //"carrier_name": "Vietnam Post",
        public string status { get; set; }
    }
}