namespace EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version_13 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Region", newName: "Regions");
            AddColumn("dbo.Customers", "DateOfFoundation", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "DateOfFoundation");
            RenameTable(name: "dbo.Regions", newName: "Region");
        }
    }
}
