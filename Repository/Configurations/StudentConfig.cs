using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configurations
{
    class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(n => n.Name).IsRequired();
            builder.Property(n => n.Name).HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);

            builder.HasData(new List<Student>
            {
                new Student
                {
                    Id = 1,
                    Name = "APK",
                    Address = "SKLM",
                    Email = "apk@gmail.com",
                    DOB = new DateTime(1997, 12, 21)
                },
                new Student
                {
                    Id = 2,
                    Name = "Poojita",
                    Address = "VIZAG",
                    Email = "poojita@gmail.com",
                    DOB = new DateTime(2000, 09, 29)
                }
            });
        }
    }
}
