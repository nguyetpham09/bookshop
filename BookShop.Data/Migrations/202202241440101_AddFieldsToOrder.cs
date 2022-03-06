namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CustomerAddressDistrict", c => c.String());
            AddColumn("dbo.Orders", "CustomerAddressCity", c => c.String());
            AddColumn("dbo.Orders", "CustomerAddressWard", c => c.String());
            AddColumn("dbo.Orders", "Weight", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "ShipmentId", c => c.String());
            AddColumn("dbo.Orders", "ShipmentStatus", c => c.String());
            AddColumn("dbo.Orders", "RateId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "RateId");
            DropColumn("dbo.Orders", "ShipmentStatus");
            DropColumn("dbo.Orders", "ShipmentId");
            DropColumn("dbo.Orders", "Weight");
            DropColumn("dbo.Orders", "CustomerAddressWard");
            DropColumn("dbo.Orders", "CustomerAddressCity");
            DropColumn("dbo.Orders", "CustomerAddressDistrict");
        }
    }
}
