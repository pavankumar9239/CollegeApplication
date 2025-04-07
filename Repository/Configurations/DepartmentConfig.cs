using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Models;

namespace Repository.Configurations
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(n => n.Name).HasMaxLength(200).IsRequired();
            builder.Property(n => n.Description).HasMaxLength(500).IsRequired(false);

            builder.HasData(new List<Department>
            {
                new Department()
                {
                    Id = 1,
                    Name = "CSE",
                    Description = "Computer Science and Engineering Department"
                },
                new Department()
                {
                    Id = 2,
                    Name = "ECE",
                    Description = "Electronic and Communications Engineering Department"
                }
            });
        }
    }
}
