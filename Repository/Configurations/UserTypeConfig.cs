using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configurations
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserTypes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1500);

            builder.HasData(new List<UserType>
            {
                new UserType()
                {
                    Id = 1,
                    Name = "Students",
                    Description = "For Students"
                },
                new UserType()
                {
                    Id = 2,
                    Name = "Teaching Staff",
                    Description = "For Teaching Staff"
                },
                new UserType()
                {
                    Id = 3,
                    Name = "Non Teaching Staff",
                    Description = "For Non Teaching Staff"
                },
                new UserType()
                {
                    Id = 4,
                    Name = "Parents",
                    Description = "For Parents"
                }
            });
        }
    }
}
