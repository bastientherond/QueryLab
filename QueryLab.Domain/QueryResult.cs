using System.Data;

namespace QueryLab.Domain;

public class QueryResult
{
    public DataTable Data { get; set; } = new();
    public int RowCount => Data.Rows.Count;
}