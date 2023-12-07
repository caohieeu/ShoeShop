namespace ProjectShoeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editShippingAddress2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingAddresses", "Address", c => c.String());
            DropColumn("dbo.ShippingAddresses", "AddressLine");
            DropColumn("dbo.ShippingAddresses", "City");
            DropColumn("dbo.ShippingAddresses", "District");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShippingAddresses", "District", c => c.String());
            AddColumn("dbo.ShippingAddresses", "City", c => c.String());
            AddColumn("dbo.ShippingAddresses", "AddressLine", c => c.String());
            DropColumn("dbo.ShippingAddresses", "Address");
        }
    }
}
