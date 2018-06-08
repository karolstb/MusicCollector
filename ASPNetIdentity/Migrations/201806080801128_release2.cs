namespace MusicCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class release2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Releases", "RecordCompany", c => c.String());
            AddColumn("dbo.Releases", "ReleaseCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Releases", "ReleaseCode");
            DropColumn("dbo.Releases", "RecordCompany");
        }
    }
}
