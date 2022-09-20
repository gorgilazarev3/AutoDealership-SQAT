namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedVehicleReservationModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VehicleReservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        IsTestDrive = c.Boolean(nullable: false),
                        ReservedUntil = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VehicleReservations");
        }
    }
}
