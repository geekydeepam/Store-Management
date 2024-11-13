namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRequi : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ProductListForBills");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductListForBills",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        fk_custId = c.Int(nullable: false),
                        Fk_ProductId = c.Int(nullable: false),
                        productQuantity = c.Double(nullable: false),
                        price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
    }
}
