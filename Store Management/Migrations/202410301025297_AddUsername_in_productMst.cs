namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsername_in_productMst : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductMsts", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductMsts", "Username");
        }
    }
}
