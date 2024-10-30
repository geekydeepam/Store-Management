namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerMst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerMsts",
                c => new
                    {
                        pk_CustId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Username = c.String(),
                        MobNo = c.String(),
                    })
                .PrimaryKey(t => t.pk_CustId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomerMsts");
        }
    }
}
