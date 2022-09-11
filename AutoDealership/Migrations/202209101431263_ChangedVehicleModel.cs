namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedVehicleModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "Brand_Id", "dbo.Brands");
            DropIndex("dbo.Vehicles", new[] { "Brand_Id" });
            RenameColumn(table: "dbo.Vehicles", name: "Brand_Id", newName: "BrandId");
            AlterColumn("dbo.Vehicles", "BrandId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vehicles", "BrandId");
            AddForeignKey("dbo.Vehicles", "BrandId", "dbo.Brands", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "BrandId", "dbo.Brands");
            DropIndex("dbo.Vehicles", new[] { "BrandId" });
            AlterColumn("dbo.Vehicles", "BrandId", c => c.Int());
            RenameColumn(table: "dbo.Vehicles", name: "BrandId", newName: "Brand_Id");
            CreateIndex("dbo.Vehicles", "Brand_Id");
            AddForeignKey("dbo.Vehicles", "Brand_Id", "dbo.Brands", "Id");
        }
    }
}
