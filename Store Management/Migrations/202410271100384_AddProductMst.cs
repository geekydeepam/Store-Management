namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductMst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductMsts",
                c => new
                    {
                        pk_ProductID = c.Int(nullable: false, identity: true),
                        fk_ProductID = c.Int(nullable: false),
                        ProductName = c.String(),
                        OriginalPrice = c.Double(nullable: false),
                        SellingPrice = c.Double(nullable: false),
                        ProductQuantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.pk_ProductID);
           
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductMsts");
        }
    }
}
