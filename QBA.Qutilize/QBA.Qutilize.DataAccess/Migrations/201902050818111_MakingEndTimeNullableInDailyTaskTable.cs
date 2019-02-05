namespace QBA.Qutilize.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingEndTimeNullableInDailyTaskTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyTasks", "EndDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyTasks", "EndDateTime", c => c.DateTime(nullable: false));
        }
    }
}
