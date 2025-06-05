namespace QueryLab.Domain;

public class DatabaseConnectionInfo
{
    public string Name { get; set; } = "";
    public string Provider { get; set; } = "";  // Ex: "SqlServer", "PostgreSQL", "Oracle", etc.
    public string ConnectionString { get; set; } = "";
}