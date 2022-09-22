namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedImagesURLTypeInVehicleModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "ImagesURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "ImagesURL");
        }
    }
}
