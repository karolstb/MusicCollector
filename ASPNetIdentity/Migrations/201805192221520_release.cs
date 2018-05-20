namespace MusicCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class release : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Releases",
                c => new
                    {
                        EntryNo = c.Int(nullable: false, identity: true),
                        AlbumNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EntryNo)
                .ForeignKey("dbo.Albums", t => t.AlbumNo, cascadeDelete: true)
                .Index(t => t.AlbumNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Releases", "AlbumNo", "dbo.Albums");
            DropIndex("dbo.Releases", new[] { "AlbumNo" });
            DropTable("dbo.Releases");
        }
    }
}
