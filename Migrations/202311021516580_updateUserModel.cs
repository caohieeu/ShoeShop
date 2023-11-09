namespace ProjectShoeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Birth", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Birth");
        }
    }
}
