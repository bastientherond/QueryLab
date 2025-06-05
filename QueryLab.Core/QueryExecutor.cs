using System.Data;
using System.Data.Common;
using QueryLab.Domain;

namespace QueryLab.Core;

public class QueryExecutor : IQueryExecutor
{
    public async Task<QueryResult> ExecuteAsync(string sql, DatabaseConnectionInfo connectionInfo, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(connectionInfo.Provider))
            throw new ArgumentException("Provider is not specified in connection info.");

        var factory = DbProviderFactories.GetFactory(connectionInfo.Provider);

        await using var connection = factory.CreateConnection();
        if (connection == null)
            throw new InvalidOperationException("Unable to create connection.");

        connection.ConnectionString = connectionInfo.ConnectionString;

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var result = new QueryResult
        {
            Data = new DataTable()
        };

        result.Data.Load(reader);

        // ✅ Normalisation des colonnes après chargement
        NormalizeColumnNames(result.Data);

        return result;
    }

    // Ajoute ce helper ici même (ou mieux dans un formatter réutilisable plus tard)
    private void NormalizeColumnNames(DataTable dataTable)
    {
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            var column = dataTable.Columns[i];
            if (string.IsNullOrWhiteSpace(column.ColumnName) || column.ColumnName == "?column?")
            {
                column.ColumnName = $"Column{i + 1}";
            }
        }
    }
}