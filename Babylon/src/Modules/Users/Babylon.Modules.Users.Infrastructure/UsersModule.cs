﻿using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Users.Application;
using Babylon.Modules.Users.Application.Abstractions.Data;
using Babylon.Modules.Users.Domain.Users;
using Babylon.Modules.Users.Infrastructure.Database;
using Babylon.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);

        return services;
    }
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connection = configuration.GetConnectionString("DataBase");

        services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(connection,
            options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users))
        .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

