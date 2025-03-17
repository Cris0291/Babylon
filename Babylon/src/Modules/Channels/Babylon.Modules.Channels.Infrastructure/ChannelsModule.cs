using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Modules.Channels.Infrastructure;
public static class ChannelsModule
{
    public static IServiceCollection AddChannelsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        return services;
    }
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ChannelsDbContext>(options => options.UseSqlServer(databaseConnectionString, 
            options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Channels)));

        return services;
    }
}
