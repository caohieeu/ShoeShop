namespace ProjectShoeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGenderShoeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "GenderShoe", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "GenderShoe");
        }
    }
}
