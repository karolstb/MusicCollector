namespace MusicCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photo1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        EntryNo = c.Int(nullable: false, identity: true),
                        AlbumNo = c.Int(),
                        ReleaseNo = c.Int(),
                        FilePath = c.String(),
                        UserUploaded = c.String(),
                    })
                .PrimaryKey(t => t.EntryNo)
                .ForeignKey("dbo.Albums", t => t.AlbumNo)
                .ForeignKey("dbo.Releases", t => t.ReleaseNo)
                .Index(t => t.AlbumNo)
                .Index(t => t.ReleaseNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "ReleaseNo", "dbo.Releases");
            DropForeignKey("dbo.Photos", "AlbumNo", "dbo.Albums");
            DropIndex("dbo.Photos", new[] { "ReleaseNo" });
            DropIndex("dbo.Photos", new[] { "AlbumNo" });
            DropTable("dbo.Photos");
        }
    }
}
