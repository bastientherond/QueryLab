namespace QueryLab.Domain;

public class SqlEditorDocument
{
    public string Title { get; set; } = "Nouveau script";

    public string? FilePath { get; set; }

    public string SqlText { get; set; } = "";

    public DatabaseConnectionInfo? Connection { get; set; }

    public QueryResult? Result { get; set; }

    public string? ErrorMessage { get; set; }

    public TimeSpan? ExecutionTime { get; set; }

    public bool IsDirty { get; private set; } = false;

    public string TabTitle => IsDirty ? $"*{Title}" : Title;

    public void MarkDirty() => IsDirty = true;

    public void MarkClean() => IsDirty = false;

    public void LoadFromFile(string path)
    {
        SqlText = File.ReadAllText(path);
        FilePath = path;
        Title = Path.GetFileName(path);
        IsDirty = false;
    }

    public void SaveToFile()
    {
        if (string.IsNullOrEmpty(FilePath))
            throw new InvalidOperationException("Aucun fichier associé.");
        File.WriteAllText(FilePath, SqlText);
        IsDirty = false;
    }

    public void SaveAs(string path)
    {
        File.WriteAllText(path, SqlText);
        FilePath = path;
        Title = Path.GetFileName(path);
        IsDirty = false;
    }
}

