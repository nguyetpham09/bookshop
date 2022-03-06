namespace BookShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentFiled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderAmount", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "CodFee", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Total", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Total");
            DropColumn("dbo.Orders", "CodFee");
            DropColumn("dbo.Orders", "OrderAmount");
        }
    }
}
