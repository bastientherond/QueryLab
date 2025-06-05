namespace QueryLab.Domain;

public class SqlEditorDocument
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public string SqlText { get; set; } = "";

    public DatabaseConnectionInfo? Connection { get; set; }

    public QueryResult? Result { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan? ExecutionTime { get; set; }
}