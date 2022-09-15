namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedUserAndVehicleModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "IsTestDriven", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vehicles", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "ReservedVehicle_Id", c => c.Int());
            CreateIndex("dbo.Vehicles", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "ReservedVehicle_Id");
            AddForeignKey("dbo.AspNetUsers", "ReservedVehicle_Id", "dbo.Vehicles", "Id");
            AddForeignKey("dbo.Vehicles", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ReservedVehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.AspNetUsers", new[] { "ReservedVehicle_Id" });
            DropIndex("dbo.Vehicles", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "ReservedVehicle_Id");
            DropColumn("dbo.Vehicles", "ApplicationUser_Id");
            DropColumn("dbo.Vehicles", "IsTestDriven");
        }
    }
}
