namespace ProjectShoeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editOrderAndDiscountModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "DiscountID", "dbo.Discounts");
            DropIndex("dbo.Orders", new[] { "DiscountID" });
            AddColumn("dbo.Discounts", "ProductId", c => c.String(maxLength: 128));
            AddColumn("dbo.Orders", "DeliveryStatus", c => c.String());
            CreateIndex("dbo.Discounts", "ProductId");
            AddForeignKey("dbo.Discounts", "ProductId", "dbo.Products", "Id");
            DropColumn("dbo.Orders", "DiscountID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "DiscountID", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Discounts", "ProductId", "dbo.Products");
            DropIndex("dbo.Discounts", new[] { "ProductId" });
            DropColumn("dbo.Orders", "DeliveryStatus");
            DropColumn("dbo.Discounts", "ProductId");
            CreateIndex("dbo.Orders", "DiscountID");
            AddForeignKey("dbo.Orders", "DiscountID", "dbo.Discounts", "Id");
        }
    }
}
