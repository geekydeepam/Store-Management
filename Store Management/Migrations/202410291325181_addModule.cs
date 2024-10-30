namespace Store_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModuleMsts",
                c => new
                    {
                        pk_moduleID = c.Int(nullable: false, identity: true),
                        ModuleName = c.String(),
                        ModuleDesc = c.String(),
                        ActionName = c.String(),
                        ControllerName = c.String(),
                        AreaName = c.String(),
                        ImageUrl = c.String(),
                        IsActive = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.pk_moduleID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ModuleMsts");
        }
    }
}
