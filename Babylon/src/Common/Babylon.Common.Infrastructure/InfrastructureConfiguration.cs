using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Babylon.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        SqlClientFactory sqlDataSource = SqlClientFactory.Instance;
        DbDataSource sqlDbSource = sqlDataSource.CreateDataSource(connectionString);
        services.TryAddSingleton(sqlDbSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        return services;
    }
}
    

