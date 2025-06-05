using QueryLab.Domain;

namespace QueryLab.Core;

public interface IQueryExecutor
{
    Task<QueryResult> ExecuteAsync(string sql, DatabaseConnectionInfo connectionInfo, CancellationToken cancellationToken);
}