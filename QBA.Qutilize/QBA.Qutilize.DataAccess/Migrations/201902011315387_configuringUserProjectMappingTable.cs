namespace QBA.Qutilize.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class configuringUserProjectMappingTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserProjects", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.UserProjects", name: "Project_Id", newName: "ProjectId");
            RenameIndex(table: "dbo.UserProjects", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.UserProjects", name: "IX_Project_Id", newName: "IX_ProjectId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserProjects", name: "IX_ProjectId", newName: "IX_Project_Id");
            RenameIndex(table: "dbo.UserProjects", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.UserProjects", name: "ProjectId", newName: "Project_Id");
            RenameColumn(table: "dbo.UserProjects", name: "UserId", newName: "User_Id");
        }
    }
}
