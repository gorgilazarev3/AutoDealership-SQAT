namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedFeaturesTypeInVehicleModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Features", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "Features");
        }
    }
}
