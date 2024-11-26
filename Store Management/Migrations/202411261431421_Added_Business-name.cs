namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Businessname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BusinessName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BusinessName");
        }
    }
}
