using System.Data.Common;
using Babylon.Common.Application.Data;

namespace Babylon.Common.Infrastructure.Data;
internal sealed class DbConnectionFactory(DbDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
