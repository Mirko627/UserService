using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserService.Repository.Entities;
using UserService.Shared.enums;

namespace UserService.Data.Context
{
    public class UserDBContext : DbContext
    {
        public DbSet<User> users { get; set; }

        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .IsRequired()
                    .HasDefaultValue(UserType.Client);

                entity.ToTable(t => t.HasCheckConstraint(
                   name: "Ck_User_Role",
                   sql: "Role IN ('Client','Agent','Owner')"
               ));

                entity.Property(u => u.LastModified)
                      .HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
