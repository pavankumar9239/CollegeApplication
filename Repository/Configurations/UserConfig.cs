using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Models;

namespace Repository.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.PasswordSalt).IsRequired();
            builder.Property(x => x.UserTypeId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate);

            builder.HasOne(x => x.UserType)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.UserTypeId)
                .HasConstraintName("FK_Users_UserTypes");
        }
    }
}
