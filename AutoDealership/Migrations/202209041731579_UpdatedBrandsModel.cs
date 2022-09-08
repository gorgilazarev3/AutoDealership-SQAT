namespace AutoDealership.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBrandsModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LogoURL = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vehicles", "Brand_Id", c => c.Int());
            CreateIndex("dbo.Vehicles", "Brand_Id");
            AddForeignKey("dbo.Vehicles", "Brand_Id", "dbo.Brands", "Id");
            DropColumn("dbo.Vehicles", "Brand_Name");
            DropColumn("dbo.Vehicles", "Brand_LogoURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Brand_LogoURL", c => c.String());
            AddColumn("dbo.Vehicles", "Brand_Name", c => c.String());
            DropForeignKey("dbo.Vehicles", "Brand_Id", "dbo.Brands");
            DropIndex("dbo.Vehicles", new[] { "Brand_Id" });
            DropColumn("dbo.Vehicles", "Brand_Id");
            DropTable("dbo.Brands");
        }
    }
}
