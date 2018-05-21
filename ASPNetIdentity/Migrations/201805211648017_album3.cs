namespace MusicCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class album3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "Duration", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "Duration");
        }
    }
}
