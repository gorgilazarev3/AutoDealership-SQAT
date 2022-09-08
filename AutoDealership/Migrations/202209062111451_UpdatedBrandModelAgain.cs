namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBrandModelAgain : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Brands", "LogoURL", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Brands", "LogoURL", c => c.String(nullable: false));
        }
    }
}
