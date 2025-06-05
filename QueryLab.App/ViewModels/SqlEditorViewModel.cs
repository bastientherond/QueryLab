using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueryLab.App.Utils;
using QueryLab.Core;
using QueryLab.Domain;

namespace QueryLab.App.ViewModels;

public partial class SqlEditorViewModel(SqlEditorDocument document, IQueryExecutor queryExecutor, IDialogService dialogService, IStorageService storageService) : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<ObservableCollection<KeyValuePair<string, object>>> _rows = [];
    
    [ObservableProperty]
    private ObservableCollection<string> _columns = [];
    
    [ObservableProperty]
    private SqlEditorDocument _document  = document;

    [ObservableProperty]
    private bool _isExecuting;

    private CancellationTokenSource? _cancellationTokenSource;

    [RelayCommand(CanExecute = nameof(CanExecuteQuery))]
    private async Task ExecuteQueryAsync()
    {
        try
        {
            if (Document.Connection == null)
            {
                throw new Exception("Aucune connexion sélectionnée.");
            }

            if (string.IsNullOrWhiteSpace(Document.SqlText))
            {
                throw new Exception("La requête SQL est vide.");
            }
        }
        catch (Exception ex)
        {
            await dialogService.ShowErrorAsync(ex.Message);
            return;
        }

        try
        {
            IsExecuting = true;
            Document.ErrorMessage = null;
            Document.ExecutionTime = null;

            _cancellationTokenSource = new CancellationTokenSource();
            var stopwatch = Stopwatch.StartNew();

            Document.Result = await queryExecutor.ExecuteAsync(
                Document.SqlText,
                Document.Connection,
                _cancellationTokenSource.Token);

            stopwatch.Stop();
            Document.ExecutionTime = stopwatch.Elapsed;
        }
        catch (OperationCanceledException oce)
        {
            await dialogService.ShowErrorAsync(oce.Message);
            Document.ErrorMessage = "Exécution annulée.";
        }
        catch (Exception ex)
        {
            await dialogService.ShowErrorAsync(ex.Message);
            Document.ErrorMessage = ex.Message;
        }
        finally
        {
            IsExecuting = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        Rows.Clear();
        Columns.Clear();

        if (Document.Result?.Data != null)
        {
            // Génération des colonnes
            foreach (DataColumn column in Document.Result.Data.Columns)
            {
                Columns.Add(column.ColumnName);
            }

            // Génération des rows
            foreach (DataRow row in Document.Result.Data.Rows)
            {
                var kvpList = new ObservableCollection<KeyValuePair<string, object>>();
                foreach (DataColumn column in Document.Result.Data.Columns)
                {
                    kvpList.Add(new KeyValuePair<string, object>(column.ColumnName, row[column]));
                }
                Rows.Add(kvpList);
            }
        }
        OnPropertyChanged(nameof(Document));
    }

    private bool CanExecuteQuery() => !IsExecuting;

    [RelayCommand]
    private void CancelQuery()
    {
        _cancellationTokenSource?.Cancel();
    }
    
    [RelayCommand]
    private async Task Save()
    {
        if (Document.FilePath != null)
        {
            Document.SaveToFile();
            OnPropertyChanged(nameof(Document.TabTitle));
        }
        else
        {
            await SaveAsAsync();
        }
    }

    [RelayCommand]
    private async Task SaveAsAsync()
    {
        var path = await storageService.SaveFileAsync();

        if (!string.IsNullOrEmpty(path))
        {
            await File.WriteAllTextAsync(path, Document.SqlText);
            Document.FilePath = path;
            Document.Title = Path.GetFileName(path);
            Document.MarkClean();
            OnPropertyChanged(nameof(Document.TabTitle));
        }
    }
}