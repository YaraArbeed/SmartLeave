using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace SmartLeave.DAL.SeedConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Id = "639de03f-7876-4fff-96ec-37f8bd3bf180",
                Name = "Employee",
                NormalizedName = "EMPLOYEE", // Important for Identity to be uppercase
            },
            new IdentityRole
            {
                Id = "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f",
                Name = "Admin",
                NormalizedName = "ADMIN", // Important for Identity to be uppercase
            },
           new IdentityRole
           {
               Id = "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
               Name = "Manager",
               NormalizedName = "MANAGER", // Important for Identity to be uppercase
           }
        );
        }
    }
}
