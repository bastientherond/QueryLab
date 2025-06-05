using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace QueryLab.App.Utils;

public class StorageService : IStorageService
{
    private Window? _mainWindow;

    public void SetMainWindow(Window? mainWindow) => _mainWindow = mainWindow;

    public async Task<string?> SaveFileAsync()
    {
        if (_mainWindow == null) throw new NullReferenceException();
        var file = await _mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Enregistrer sous",
            SuggestedFileName = "nouveau_script.sql",
            FileTypeChoices = new List<FilePickerFileType>
            {
                new("Fichier SQL") { Patterns = new[] { "*.sql" } },
                new("Tous les fichiers") { Patterns = new[] { "*.*" } }
            }
        });

        return file?.Path.LocalPath;
    }

    public async Task<(string? Content, string? Path)> OpenFileAsync()
    {
        if (_mainWindow == null) throw new NullReferenceException();
        var files = await _mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Ouvrir un fichier SQL",
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>
            {
                new("Fichier SQL") { Patterns = new[] { "*.sql" } },
                new("Tous les fichiers") { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count == 0)
            return (null, null);

        var file = files[0];
        await using var stream = await file.OpenReadAsync();
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();

        return (content, file.Path.LocalPath);
    }
}