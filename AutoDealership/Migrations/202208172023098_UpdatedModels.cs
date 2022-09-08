namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Brand_Name = c.String(),
                        Brand_LogoURL = c.String(),
                        Model = c.String(),
                        FuelType = c.Int(nullable: false),
                        BodyStyle = c.Int(nullable: false),
                        Transmission_NumberSpeeds = c.Int(nullable: false),
                        Transmission_TransmissionType = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Mileage = c.Int(nullable: false),
                        DrivetrainType = c.Int(nullable: false),
                        Color = c.String(),
                        InteriorColor = c.String(),
                        FuelEfficiency = c.Double(nullable: false),
                        Horsepower = c.Int(nullable: false),
                        Torque = c.Int(nullable: false),
                        Engine = c.String(),
                        Description = c.String(),
                        Price = c.Int(nullable: false),
                        IsForLease = c.Boolean(nullable: false),
                        IsForRent = c.Boolean(nullable: false),
                        MonthlyPayment = c.Int(nullable: false),
                        DailyPayment = c.Int(nullable: false),
                        VehicleStatus = c.Int(nullable: false),
                        InStock = c.Boolean(nullable: false),
                        CoverImageURL = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vehicles");
        }
    }
}
