namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ReservedVehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.AspNetUsers", new[] { "ReservedVehicle_Id" });
            AddColumn("dbo.AspNetUsers", "ReservedVehicleId", c => c.Int());
            DropColumn("dbo.AspNetUsers", "ReservedVehicle_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ReservedVehicle_Id", c => c.Int());
            DropColumn("dbo.AspNetUsers", "ReservedVehicleId");
            CreateIndex("dbo.AspNetUsers", "ReservedVehicle_Id");
            AddForeignKey("dbo.AspNetUsers", "ReservedVehicle_Id", "dbo.Vehicles", "Id");
        }
    }
}
