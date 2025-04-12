using Babylon.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Users.Infrastructure.Users;
internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(100);

        builder.HasData(
            Permission.CreateChannel,
            Permission.GetChannels
            );

        builder.HasMany<Role>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("role_permissions");

                joinBuilder.HasData(
                    CreateRolePermission(Role.Member, Permission.GetChannels),
                    CreateRolePermission(Role.Administrator, Permission.CreateChannel),
                    CreateRolePermission(Role.Administrator, Permission.GetChannels)
                    );
            });
    }

    private static object CreateRolePermission(Role role, Permission permission) => new
    {
        RoleName = role.Name,
        PermissionCode = permission.Code
    };
}
