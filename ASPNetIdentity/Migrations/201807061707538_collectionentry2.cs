namespace MusicCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class collectionentry2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CollectionEntries", "HashControlValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CollectionEntries", "HashControlValue");
        }
    }
}
