namespace ITDeviceManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DeviceCategoryProperties",
                c => new
                    {
                        DeviceCategoryID = c.Int(nullable: false),
                        PropertyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DeviceCategoryID, t.PropertyID })
                .ForeignKey("dbo.DeviceCategories", t => t.DeviceCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Properties", t => t.PropertyID, cascadeDelete: true)
                .Index(t => t.DeviceCategoryID)
                .Index(t => t.PropertyID);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PropertyDescription = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DeviceProperties",
                c => new
                    {
                        DeviceID = c.Int(nullable: false),
                        PropertyID = c.Int(nullable: false),
                        PropertyValue = c.String(),
                    })
                .PrimaryKey(t => new { t.DeviceID, t.PropertyID })
                .ForeignKey("dbo.Devices", t => t.DeviceID, cascadeDelete: true)
                .ForeignKey("dbo.Properties", t => t.PropertyID, cascadeDelete: true)
                .Index(t => t.DeviceID)
                .Index(t => t.PropertyID);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeviceName = c.String(),
                        DeviceCategoryID = c.Int(nullable: false),
                        AcquisitionDate = c.DateTime(),
                        Memo = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DeviceCategories", t => t.DeviceCategoryID, cascadeDelete: true)
                .Index(t => t.DeviceCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceProperties", "PropertyID", "dbo.Properties");
            DropForeignKey("dbo.DeviceProperties", "DeviceID", "dbo.Devices");
            DropForeignKey("dbo.Devices", "DeviceCategoryID", "dbo.DeviceCategories");
            DropForeignKey("dbo.DeviceCategoryProperties", "PropertyID", "dbo.Properties");
            DropForeignKey("dbo.DeviceCategoryProperties", "DeviceCategoryID", "dbo.DeviceCategories");
            DropIndex("dbo.Devices", new[] { "DeviceCategoryID" });
            DropIndex("dbo.DeviceProperties", new[] { "PropertyID" });
            DropIndex("dbo.DeviceProperties", new[] { "DeviceID" });
            DropIndex("dbo.DeviceCategoryProperties", new[] { "PropertyID" });
            DropIndex("dbo.DeviceCategoryProperties", new[] { "DeviceCategoryID" });
            DropTable("dbo.Devices");
            DropTable("dbo.DeviceProperties");
            DropTable("dbo.Properties");
            DropTable("dbo.DeviceCategoryProperties");
            DropTable("dbo.DeviceCategories");
        }
    }
}
