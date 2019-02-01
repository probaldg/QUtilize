namespace QBA.Qutilize.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailyTasks",
                c => new
                    {
                        DailyTaskId = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        EditedBy = c.String(),
                        EditedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DailyTaskId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        ParentProjectId = c.Int(),
                        CreatedBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        EditedBy = c.String(),
                        EditedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 200),
                        Password = c.String(nullable: false, maxLength: 200),
                        Name = c.String(nullable: false, maxLength: 200),
                        CreatedBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        EditedBy = c.String(),
                        EditedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 500),
                        CreatedBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        EditedBy = c.String(),
                        EditedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Project_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.UserProjects", "User_Id", "dbo.Users");
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.UserProjects", new[] { "Project_Id" });
            DropIndex("dbo.UserProjects", new[] { "User_Id" });
            DropTable("dbo.RoleUsers");
            DropTable("dbo.UserProjects");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.DailyTasks");
        }
    }
}
