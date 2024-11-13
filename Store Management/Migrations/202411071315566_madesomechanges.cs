namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class madesomechanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillsItemTemps",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        fk_custId = c.Int(nullable: false),
                        Fk_ProductId = c.Int(nullable: false),
                        prodQuantity = c.Double(nullable: false),
                        price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BillsItemTemps");
        }
    }
}
