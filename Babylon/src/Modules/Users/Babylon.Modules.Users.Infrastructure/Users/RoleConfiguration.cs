﻿using Babylon.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Users.Infrastructure.Users;
internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Name);

        builder.Property(r => r.Name).HasMaxLength(200);

        builder.HasMany<User>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("user_roles");
                joinBuilder.Property("RolesName").HasColumnName("role_name");
            }
            );

        builder.HasData(
            Role.Administrator,
            Role.Member
            );
    }
}
