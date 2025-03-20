using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Infrastructure.Channels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Babylon.Modules.Channels.Infrastructure.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Modules.Channels.Infrastructure;
public static class ChannelsModule
{
    public static IServiceCollection AddChannelsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);

        return services;
    }
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ChannelsDbContext>(options => options.UseSqlServer(databaseConnectionString, 
            options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Channels)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ChannelsDbContext>());
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();

        return services;
    }
}
