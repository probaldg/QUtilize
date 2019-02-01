namespace QBA.Qutilize.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingEditedDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyTasks", "EditedDate", c => c.DateTime());
            AlterColumn("dbo.Projects", "EditedDate", c => c.DateTime());
            AlterColumn("dbo.Users", "EditedDate", c => c.DateTime());
            AlterColumn("dbo.Roles", "EditedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Roles", "EditedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "EditedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "EditedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DailyTasks", "EditedDate", c => c.DateTime(nullable: false));
        }
    }
}
