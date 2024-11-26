namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Owner_Full_Name_and_Address : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "OwnerName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "OwnerName");
        }
    }
}
