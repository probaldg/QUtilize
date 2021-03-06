USE [DB_A05603_qUtilize]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 03/02/2019 11:37:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DailyTasks]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DailyTasks](
	[DailyTaskId] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NULL,
	[TaskName] [nvarchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[EditedBy] [nvarchar](max) NULL,
	[EditedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.DailyTasks] PRIMARY KEY CLUSTERED 
(
	[DailyTaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Department]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CODE] [nvarchar](50) NULL,
	[NAME] [nvarchar](50) NULL,
	[DESCRIPTION] [nvarchar](50) NULL,
	[ORGID] [int] NULL,
	[isACTIVE] [bit] NULL,
	[ADDEDBY] [int] NULL,
	[EDITEDBY] [int] NULL,
	[ADDEDTS] [datetime] NULL,
	[EDITEDTS] [datetime] NULL,
 CONSTRAINT [PK_tblDepartment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Module]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[GroupName] [nvarchar](200) NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayName] [nvarchar](200) NULL,
	[DisplayICON] [nvarchar](100) NULL,
	[DisplayCSS] [nvarchar](max) NULL,
	[URL] [nvarchar](500) NULL,
	[OrgID] [int] NULL,
	[Rank] [int] NULL,
	[AddedBy] [int] NULL,
	[EditedBy] [int] NULL,
	[AddedTS] [datetime] NULL CONSTRAINT [DF_tblSYSModule_AddedTS]  DEFAULT (getdate()),
	[EditedTS] [datetime] NULL,
 CONSTRAINT [PK_tblSYSModule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Organisation]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organisation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[orgname] [nvarchar](150) NULL,
	[address] [nvarchar](150) NULL,
	[url] [nvarchar](50) NULL,
	[contact_email_id] [nvarchar](150) NULL,
	[logo] [nvarchar](150) NULL,
	[wikiurl] [nvarchar](150) NULL,
	[isActive] [bit] NULL,
	[createdTS] [datetime] NULL,
	[createdBy] [int] NULL,
	[editedTS] [datetime] NULL,
	[editedBy] [int] NULL,
 CONSTRAINT [PK_tblOrganisation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Projects]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ParentProjectId] [int] NULL,
	[DeptID] [int] NULL,
	[PMUserID] [int] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[EditedBy] [nvarchar](max) NULL,
	[EditedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[MaxProjectTimeInHours] [int] NULL,
 CONSTRAINT [PK_dbo.Projects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RoleModuleMap]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleModuleMap](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[SYSModuleID] [int] NOT NULL,
	[AddedBy] [int] NULL,
	[EditedBy] [int] NULL,
	[AddedTS] [datetime] NULL,
	[EditedTS] [datetime] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_tblRoleSysModuleMap] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[EditedBy] [nvarchar](max) NULL,
	[EditedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProjects]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProjects](
	[UserId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserProjects] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[RoleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](200) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[DeptID] [int] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[EditedBy] [nvarchar](max) NULL,
	[EditedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[EmailId] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_Department_Organisation] FOREIGN KEY([ORGID])
REFERENCES [dbo].[Organisation] ([id])
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Organisation]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Department] FOREIGN KEY([DeptID])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Department]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Users] FOREIGN KEY([PMUserID])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Users]
GO
ALTER TABLE [dbo].[RoleModuleMap]  WITH CHECK ADD  CONSTRAINT [FK_RoleModuleMap_Module] FOREIGN KEY([SYSModuleID])
REFERENCES [dbo].[Module] ([ID])
GO
ALTER TABLE [dbo].[RoleModuleMap] CHECK CONSTRAINT [FK_RoleModuleMap_Module]
GO
ALTER TABLE [dbo].[RoleModuleMap]  WITH CHECK ADD  CONSTRAINT [FK_RoleModuleMap_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[RoleModuleMap] CHECK CONSTRAINT [FK_RoleModuleMap_Roles]
GO
ALTER TABLE [dbo].[UserProjects]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserProjects_dbo.Projects_Project_Id] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserProjects] CHECK CONSTRAINT [FK_dbo.UserProjects_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[UserProjects]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserProjects_dbo.Users_User_Id] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserProjects] CHECK CONSTRAINT [FK_dbo.UserProjects_dbo.Users_User_Id]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RoleUsers_dbo.Roles_Role_Id] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_dbo.RoleUsers_dbo.Roles_Role_Id]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RoleUsers_dbo.Users_User_Id] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_dbo.RoleUsers_dbo.Users_User_Id]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Department] FOREIGN KEY([DeptID])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Department]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDashBoardMenu]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetDashBoardMenu]-- 2
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/****** Script for SelectTopNRows command from SSMS  ******/
    SELECT [ID] ,[ParentID] ,[Name] ,[Description] ,[DisplayName] ,[DisplayICON] ,[DisplayCSS] ,[URL] ,[Rank] ,[AddedBy] ,[EditedBy] ,[AddedTS] ,[EditedTS]  
    FROM Module Where id in (select SYSModuleID from RoleModuleMap where roleID in (select RoleID from UserRoles where userID=@UserID))
END














GO
/****** Object:  StoredProcedure [dbo].[USP_Dashboard_Get]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[USP_Dashboard_Get] --2,'2019-02-18','2019-02-27'
    @UserID int
   ,@StartDate date  
   ,@EndDate date  
AS      
BEGIN    
	select projectID, (select name from projects where ID=projectID) as projectName ,convert(varchar,  startdatetime, 106) as Date
    , sum( DATEDIFF(second, startdatetime, isnull( enddatetime,startdatetime))) as totalSec
    ,( convert(varchar(5),sum(DateDiff(s, startdatetime, isnull( enddatetime,startdatetime)))/3600)+':'+convert(varchar(5),sum(DateDiff(s, startdatetime, isnull( enddatetime,startdatetime)))%3600/60)+':'+convert(varchar(5),sum(DateDiff(s, startdatetime, isnull( enddatetime,startdatetime)))%60)) as [hms]
	from dailytasks
	where userID=@UserID AND  convert(date,startdatetime,106)>= @StartDate AND convert(date,startdatetime,106)<=@EndDate
	group by projectID, convert(varchar,startdatetime,106)
	order by convert(varchar,startdatetime,106), projectID

	 select projectID , (select name from projects where ID=projectID) as projectName
    , sum( DATEDIFF(second, startdatetime, isnull( enddatetime,startdatetime))) as totalSec
	from dailytasks
	where userID=@UserID AND  convert(date,startdatetime,106)>= @StartDate AND convert(date,startdatetime,106)<=@EndDate
	group by projectID
END 

GO
/****** Object:  StoredProcedure [dbo].[USPDailyTask_Get]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create  PROC [dbo].[USPDailyTask_Get]  -- USPDailyTaskGet 11
 @DailyTaskID int = 0

AS
BEGIN
	DECLARE @QueryString nvarchar(Max)

	SET @QueryString= N'SELECT  dt.DailyTaskId, dt.UserID, dt.ProjectID, dt.StartDateTime, dt.EndDateTime, dt.CreatedBy, dt.CreateDate,dt.IsActive FROM dbo.DailyTasks dt'

	if(@DailyTaskID > 0)
	BEGIN
		set @QueryString = @QueryString  + ' Where dt.DailyTaskId = ' + CONVERT(varchar(100), @DailyTaskID )
	end
	
	EXEC (@QueryString)

END


GO
/****** Object:  StoredProcedure [dbo].[USPDailyTask_UpdateEndTaskTime]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC	[dbo].[USPDailyTask_UpdateEndTaskTime]
	@DailyTaskId int,
	@StartDateTime datetime	
AS
BEGIN

UPDATE [DailyTasks]

   SET [EndDateTime] = @StartDateTime    WHERE DailyTaskId	=@DailyTaskId

END



GO
/****** Object:  StoredProcedure [dbo].[USPDailyTasks_InsertTaskStartTime]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC	[dbo].[USPDailyTasks_InsertTaskStartTime] -- USPDailyTasks_InsertTaskStartTime 1,5,'2019-02-08 17:02:49.510',1,1
	@UserID int,
	@ProjectId int,
	@StartDateTime datetime	,
	@Createdby nvarchar(10),
	@IsActive bit	
	
AS
BEGIN 

INSERT INTO [dbo].[DailyTasks]
           ([UserID]
           ,[ProjectID]
           ,[StartDateTime]
           ,[CreatedBy]
           ,[CreateDate]
           ,[IsActive])
     VALUES
           (@UserID	
           ,@ProjectId
           ,@StartDateTime
           ,@UserID	
           ,@StartDateTime
           ,@IsActive)

select SCOPE_IDENTITY() AS [DailyTaskId]

END  

GO
/****** Object:  StoredProcedure [dbo].[USPModules_Get]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USPModules_Get]  
 @Id int= null  
AS  
BEGIN   
 if(@Id is null)  
 begin  
  SELECT m.ID ,m.ParentID,m.GroupName,m.Name,m.Description,m.DisplayName,M.DisplayICON,  
  m.DisplayCSS,m.URL,m.OrgID,m.Rank,m.AddedBy,m.AddedTS  
    FROM dbo.Module m    
 END  
 else  
 BEGIN  
 SELECT m.ID ,m.ParentID,m.GroupName,m.Name,m.Description,m.DisplayName,M.DisplayICON,  
  m.DisplayCSS,m.URL,m.OrgID,m.Rank,m.AddedBy,m.AddedTS  
    FROM dbo.Module m  WHERE m.ID=@id  
 end  
  
end  
GO
/****** Object:  StoredProcedure [dbo].[USPProjecs_Insert]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USPProjecs_Insert]
	@Name nvarchar(	50),
	@Description nvarchar(	500)= null,
	@ParentProjectId int =null,
	@CreatedBy nvarchar(100)= null,
	@createdDate datetime,
	@IsActive bit,	
	@MaxProjectTimeInHours int=8,
	@Identity int OUTPUT
AS
BEGIN
	INSERT INTO dbo.Projects
(
    Name,
    Description,
    ParentProjectId,
    CreatedBy,
    CreateDate,
    IsActive,
    MaxProjectTimeInHours
)
VALUES
(
   @Name,
   @Description,
   @ParentProjectId,
   @CreatedBy,
   @createdDate,
   @IsActive,
   @MaxProjectTimeInHours
)

SET @Identity = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[USPProjects_Get]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROC	[dbo].[USPProjects_Get]
	@Id int =0
AS
  BEGIN
	if(@Id = 0)
	begin
		SELECT p.Id, p.Name, p.Description, p.ParentProjectId,
		(SELECT Name FROM Projects WHERE Id= p.ParentProjectId) AS ParentProjectName,
		 p.CreateDate, p.EditedBy, p.EditedDate, p.IsActive FROM dbo.Projects p WHERE p.IsActive  =1
	END
else
	BEGIN
			SELECT p.Id, p.Name, p.Description, p.ParentProjectId, 
			(SELECT Name FROM Projects WHERE Id= p.ParentProjectId) AS ParentProjectName,
			p.CreateDate, p.EditedBy, p.EditedDate, p.IsActive FROM dbo.Projects p 
			WHERE p.IsActive  =1 AND p.Id= @id
	END
   END
GO
/****** Object:  StoredProcedure [dbo].[USPProjects_Update]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROC	[dbo].[USPProjects_Update]
@Id int,
@Name nvarchar(	100),
@Description nvarchar(	500),
@ParentProjectId int= NULL	,
@EditedBy nvarchar(	100)= null,
@EditedDate datetime	,
@IsActive bit,
@MaxProjectTimeInHours int

AS
	BEGIN
	UPDATE dbo.Projects
SET
    --Id - this column value is auto-generated
    Name = @Name,
    [Description]= @Description	,
    ParentProjectId = @ParentProjectId,
    EditedBy =@EditedBy,
    EditedDate = @EditedDate,
    IsActive =@IsActive,
    MaxProjectTimeInHours =@MaxProjectTimeInHours

	where Id= @Id
END


GO
/****** Object:  StoredProcedure [dbo].[USPRoleModuleMap_DeleteMappingByRoleID]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USPRoleModuleMap_DeleteMappingByRoleID]
	@RoleId int
AS
BEGIN
	Delete FROM RoleModuleMap 
		 WHERE RoleID =@RoleId

END
GO
/****** Object:  StoredProcedure [dbo].[USPRoleModuleMap_GetModuleByID]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USPRoleModuleMap_GetModuleByID]
	@RoleID int
AS
BEGIN
	SELECT	rmm.ID, rmm.RoleID, rmm.SYSModuleID, rmm.AddedBy, rmm.EditedBy, rmm.Active FROM dbo.RoleModuleMap rmm	WHERE rmm.RoleID=@RoleID
end
GO
/****** Object:  StoredProcedure [dbo].[USPRoleMouduleMapping_Insert]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USPRoleMouduleMapping_Insert]
	@RoleID int,
	@SysModuleId int,
	@AddedBy int= null,
	@AddedTS datetime	
AS
BEGIN
	INSERT	INTO dbo.RoleModuleMap
	(
	    --ID - this column value is auto-generated
	    RoleID,
	    SYSModuleID,
	    AddedBy,
	    AddedTS
	  
	)
	VALUES
	(
	   @RoleID,@SysModuleId,@AddedBy,@AddedTS
	   
	)
END
GO
/****** Object:  StoredProcedure [dbo].[USPRoles_Get]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USPRoles_Get]
	@Id int=0
AS
BEGIN
if(@Id = 0)
	begin
		SELECT	r.Id, r.Name, r.Description, r.CreatedBy, r.CreateDate, r.IsActive FROM dbo.Roles r
		WHERE r.IsActive= 1
	END
else
	BEGIN
		SELECT	r.Id, r.Name, r.Description, r.CreatedBy, r.CreateDate, r.IsActive FROM dbo.Roles r
		WHERE r.IsActive= 1 AND r.Id= @id
	END
	

	
end
GO
/****** Object:  StoredProcedure [dbo].[USPRoles_Insert]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC	[dbo].[USPRoles_Insert]
	@Name nvarchar(100),
	@Description nvarchar(	150),
	@CreatedBy nvarchar(	100) = null,
	@CreatedDate datetime	,
	@IsActive bit,
	 @Identity int OUTPUT
AS
BEGIN
	INSERT INTO dbo.Roles
 (  
    Name,
    Description,
    CreatedBy,
    CreateDate,
    IsActive
)
VALUES
(
  
   @Name,@Description	,@CreatedBy	,@CreatedDate, @IsActive	
)

SET @Identity = SCOPE_IDENTITY()

END


GO
/****** Object:  StoredProcedure [dbo].[USPRoles_Update]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROC	[dbo].[USPRoles_Update]
	@Id int,
	@Name nvarchar(	100),
	@Description nvarchar(	150)= null,
	@EditedBy nvarchar(	100) = null,
	@EditedDate datetime	,
	@IsActive bit
AS
BEGIN
	UPDATE	Roles
SET
  
    Name =@Name,
    Description =@Description, 
    EditedBy = @EditedBy,
    EditedDate = @EditedDate,
    IsActive = @IsActive

	WHERE Id=@Id
END



GO
/****** Object:  StoredProcedure [dbo].[USPUser_Insert]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USPUser_Insert]
  @UserName nvarchar(100),
  @Password nvarchar(200),
  @Name nvarchar(	150),
  @EmailId nvarchar(	100),
  @CreatedBy nvarchar(	100) = null,
  @CreatedDate datetime	,
  @Identity int OUTPUT
AS
BEGIN

INSERT INTO dbo.Users
(
    UserName, [Password], [Name],CreatedBy, CreateDate, IsActive,EmailId
)
VALUES
(
    @UserName	,    @Password	,   @Name,    @CreatedBy,    @CreatedDate,    1,     @EmailId
)
	
SET @Identity = SCOPE_IDENTITY()
	
END
GO
/****** Object:  StoredProcedure [dbo].[USPUser_Update]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROC [dbo].[USPUser_Update]
  @ID int,
  @Name nvarchar(	150),
  @userName nvarchar(	100),
  @EmailId nvarchar(	100),
  @EditedBy nvarchar(	100)= null,
  @EditedDate datetime,
  @isActive	bit
AS
BEGIN
	UPDATE Users
	SET
	    Name = @Name,
		UserName=@userName,
	    EmailId = @EmailId,
	    EditedBy =@EditedBy,
	    EditedDate =  @EditedDate ,
	    IsActive = @isActive
		WHERE dbo.Users.Id= @ID
END
GO
/****** Object:  StoredProcedure [dbo].[USPUser_UpdatePassword]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC	 [dbo].[USPUser_UpdatePassword]
	@Id int,
	@password  nvarchar(200),
	@EditedBy nvarchar(100)= null,
	@EditedDate datetime
AS
BEGIN
	UPDATE Users
	SET
	
	    Password = @password,
	    EditedBy = @EditedBy,
	    EditedDate = @EditedDate

		where Id=@Id
	  
end
GO
/****** Object:  StoredProcedure [dbo].[USPUserDetailData]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC	[dbo].[USPUserDetailData]
	@Id int =0
AS
BEGIN

		SELECT Id, UserName, Name,isnull(EmailId,'') as EmailId, IsActive ,'' as EmployeeCode,  '' as Designation, '' as ManagerName, '' as ManagerEmpCode
		FROM dbo.Users u
		WHERE IsActive= 1 AND Id= @id
end
GO
/****** Object:  StoredProcedure [dbo].[USPUserProjects_DeleteMappingByUserID]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROC	[dbo].[USPUserProjects_DeleteMappingByUserID]
	@UserID int
  AS
  BEGIN 
	 DELETE FROM dbo.UserProjects	WHERE UserId=@UserID
  end
GO
/****** Object:  StoredProcedure [dbo].[USPUserProjects_Get]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USPUserProjects_Get]
	@UserID int
AS
BEGIN
	SELECT up.UserId, up.ProjectId FROM dbo.UserProjects up WHERE up.UserId	=@UserID
end


GO
/****** Object:  StoredProcedure [dbo].[USPUserProjects_Insert]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC	[dbo].[USPUserProjects_Insert]
	@UserID int,
	@ProjectID int
AS
BEGIN
	INSERT INTO	dbo.UserProjects
(
    UserId,
    ProjectId
)
VALUES
(
  @UserID	,@ProjectID

)
END


GO
/****** Object:  StoredProcedure [dbo].[USPUserRole_DeleteMappingByUserID]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USPUserRole_DeleteMappingByUserID]
	@UserID int
AS
BEGIN
	DELETE FROM UserRoles WHERE UserID=@UserID	
END
GO
/****** Object:  StoredProcedure [dbo].[USPUserRoles_DeleteMappingByUserID]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USPUserRoles_DeleteMappingByUserID]
	@UserId int
AS
BEGIN
	DELETE	 FROM dbo.UserRoles WHERE UserId=@UserId
end
GO
/****** Object:  StoredProcedure [dbo].[USPUserRoles_Get]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[USPUserRoles_Get]
	@UserID int
AS
BEGIN
	SELECT  ur.UserId,ur.RoleId FROM dbo.UserRoles ur WHERE ur.UserId	=@UserID
end


GO
/****** Object:  StoredProcedure [dbo].[USPUserRoles_Insert]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC	[dbo].[USPUserRoles_Insert]
	@UserID int,
	@RoleID int
AS
BEGIN
	INSERT INTO	dbo.UserRoles
(
    UserId,
    RoleId
)
VALUES
(
  @UserID	,@RoleID

)
END


GO
/****** Object:  StoredProcedure [dbo].[USPUsers_CheckEmail]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROC	[dbo].[USPUsers_CheckEmail]
	@Email nvarchar(50)
AS
BEGIN
	SELECT u.EmailId FROM dbo.Users u WHERE  u.EmailId= @Email
END
GO
/****** Object:  StoredProcedure [dbo].[USPUsers_Get]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC	[dbo].[USPUsers_Get] -- 
	@UserID nvarchar(50) = NULL,
	@Password nvarchar(50)= null
AS
BEGIN
	
	DECLARE @QueryString nvarchar(Max)

	SET @QueryString = N'SELECT u.UserName,u.Id,u.Password,u.Name,u.CreatedBy,u.CreateDate
					,p.Id AS ProjectID,	p.Name AS ProjectName, p.Description AS ProjectDescription ,p.ParentProjectId,p.MaxProjectTimeInHours
					,r.Id AS RoleID, r.Name AS RoleName  
					FROM dbo.Users u	
					LEFT JOIN	dbo.UserProjects up	ON	u.Id = up.UserId
					LEFT JOIN	dbo.Projects p on up.ProjectId = p.Id
					LEFT JOIN	dbo.UserRoles ur ON u.Id = ur.UserId
					LEFT JOIN	dbo.Roles r ON r.Id = u.Id'



	if(@UserID	 IS NOT	 NULL AND	@Password	 IS NOT NULL)

		BEGIN
			

				set @QueryString = @QueryString  + ' Where u.UserName =  ''' + @UserID + ''' and u.Password = ''' + @Password +''''

		END
	
		EXEC (@QueryString)
		
end
GO
/****** Object:  StoredProcedure [dbo].[USPUsers_GetForWeb]    Script Date: 03/02/2019 11:37:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC	[dbo].[USPUsers_GetForWeb] -- 
	@Id int =0
AS
BEGIN
if(@Id = 0)
	begin
		SELECT u.Id, u.UserName, u.Name, u.EmailId, u.IsActive FROM dbo.Users u
		WHERE u.IsActive= 1 order by u.id DESC	
	END
else
	BEGIN
		SELECT u.Id, u.UserName, u.Name, u.EmailId, u.IsActive  FROM dbo.Users u
		WHERE u.IsActive= 1 AND u.Id= @id
	END
	
	
end
GO
