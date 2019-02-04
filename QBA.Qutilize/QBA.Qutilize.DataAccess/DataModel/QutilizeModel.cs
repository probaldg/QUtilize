namespace QBA.Qutilize.DataAccess.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class QutilizeModel : DbContext
    {
       
        public QutilizeModel()
            : base("name=QutilizeDBCon")
        {
        }
               
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<DailyTask> DailyTasks { get; set; }

        //public virtual DbSet<UserRoles> UserRoles { get; set; }
        //public virtual DbSet<UserProjects> UserProjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany<Role>(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(ur => {
                    ur.MapLeftKey("UserId");
                    ur.MapRightKey("RoleId");
                    ur.ToTable("UserRoles");
                });

            modelBuilder.Entity<User>()
              .HasMany<Project>(u => u.Projects)
              .WithMany(p => p.Users)
              .Map(up =>
              {
                  up.MapLeftKey("UserId");
                  up.MapRightKey("ProjectId");
                  up.ToTable("UserProjects");
              });

        }
    }

   
}