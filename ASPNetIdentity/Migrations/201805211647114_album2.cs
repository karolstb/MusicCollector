namespace MusicCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class album2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "YearOfProduction", c => c.Int(nullable: false));
            DropColumn("dbo.Albums", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Albums", "Duration", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Albums", "YearOfProduction");
        }
    }
}
