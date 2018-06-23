namespace MusicCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class collectionentry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollectionEntries",
                c => new
                    {
                        UserNo = c.String(nullable: false, maxLength: 128),
                        AlbumNo = c.Int(nullable: false),
                        ReleaseNo = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => new { t.UserNo, t.AlbumNo, t.ReleaseNo })
                .ForeignKey("dbo.Albums", t => t.AlbumNo, cascadeDelete: true)
                .ForeignKey("dbo.Releases", t => t.ReleaseNo, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserNo, cascadeDelete: true)
                .Index(t => t.UserNo)
                .Index(t => t.AlbumNo)
                .Index(t => t.ReleaseNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CollectionEntries", "UserNo", "dbo.AspNetUsers");
            DropForeignKey("dbo.CollectionEntries", "ReleaseNo", "dbo.Releases");
            DropForeignKey("dbo.CollectionEntries", "AlbumNo", "dbo.Albums");
            DropIndex("dbo.CollectionEntries", new[] { "ReleaseNo" });
            DropIndex("dbo.CollectionEntries", new[] { "AlbumNo" });
            DropIndex("dbo.CollectionEntries", new[] { "UserNo" });
            DropTable("dbo.CollectionEntries");
        }
    }
}
