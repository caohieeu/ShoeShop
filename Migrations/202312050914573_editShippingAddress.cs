namespace ProjectShoeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editShippingAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingAddresses", "AddressLine", c => c.String());
            DropColumn("dbo.ShippingAddresses", "AddressLine1");
            DropColumn("dbo.ShippingAddresses", "AddressLine2");
            DropColumn("dbo.ShippingAddresses", "PostalCode");
            DropColumn("dbo.ShippingAddresses", "Country");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShippingAddresses", "Country", c => c.String());
            AddColumn("dbo.ShippingAddresses", "PostalCode", c => c.String());
            AddColumn("dbo.ShippingAddresses", "AddressLine2", c => c.String());
            AddColumn("dbo.ShippingAddresses", "AddressLine1", c => c.String());
            DropColumn("dbo.ShippingAddresses", "AddressLine");
        }
    }
}
