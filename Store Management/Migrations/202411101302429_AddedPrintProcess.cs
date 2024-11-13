namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPrintProcess : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProcessBills",
                c => new
                    {
                        pk_processid = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        fk_custid = c.Int(nullable: false),
                        trnId = c.Int(nullable: false),
                        fk_productId = c.Int(nullable: false),
                        ProductQuantity = c.Double(nullable: false),
                        PrductCurrentPrice = c.Double(nullable: false),
                        insertedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.pk_processid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProcessBills");
        }
    }
}
