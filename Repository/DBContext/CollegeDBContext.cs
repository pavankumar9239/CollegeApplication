using Microsoft.EntityFrameworkCore;
using Repository.Configurations;
using Repository.Models;

namespace Repository.DBContext
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) 
            : base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public DbSet<UserType> UserTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region properties
            //this can also be done from Config class
            //modelBuilder.Entity<Student>().HasData(new List<Student>
            //{
            //    new Student
            //    {
            //        Id = 1,
            //        Name = "APK",
            //        Address = "SKLM",
            //        Email = "apk@gmail.com",
            //        DOB = new DateTime(1997, 12, 21)
            //    }, 
            //    new Student
            //    {
            //        Id = 2,
            //        Name = "Poojita",
            //        Address = "VIZAG",
            //        Email = "poojita@gmail.com",
            //        DOB = new DateTime(2000, 09, 29)
            //    }
            //});


            //This can also be added from EntityClasses.
            //    modelBuilder.Entity<Student>(x =>
            //    {
            //        x.Property(n => n.Name).IsRequired();
            //        x.Property(n => n.Name).HasMaxLength(250);
            //        x.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            //        x.Property(n => n.Email).IsRequired().HasMaxLength(250);
            //    });
            #endregion

            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new RolePrivilegeConfig());
            modelBuilder.ApplyConfiguration(new UserRoleMappingConfig());
            modelBuilder.ApplyConfiguration(new UserTypeConfig());
        }
    }
}
