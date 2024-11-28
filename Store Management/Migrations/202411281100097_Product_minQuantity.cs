namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_minQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductMsts", "MinQuantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductMsts", "MinQuantity");
        }
    }
}
