using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueryLab.Domain;

namespace QueryLab.App.ViewModels;

public partial class SqlEditorViewModel(SqlEditorDocument document) : ViewModelBase
{
    [ObservableProperty]
    private SqlEditorDocument _document  = document;

    [ObservableProperty]
    private bool _isExecuting;

    private CancellationTokenSource? _cancellationTokenSource;

    [RelayCommand(CanExecute = nameof(CanExecuteQuery))]
    private async Task ExecuteQueryAsync()
    {
        if (Document.Connection == null)
        {
            Document.ErrorMessage = "Aucune connexion sélectionnée.";
            OnPropertyChanged(nameof(Document));
            return;
        }

        try
        {
            IsExecuting = true;
            Document.ErrorMessage = null;
            Document.ExecutionTime = null;

            _cancellationTokenSource = new CancellationTokenSource();
            var stopwatch = Stopwatch.StartNew();

            // Ici tu brancheras ton vrai QueryExecutor plus tard.
            await Task.Delay(1000, _cancellationTokenSource.Token);
            Document.Result = new QueryResult();

            stopwatch.Stop();
            Document.ExecutionTime = stopwatch.Elapsed;
        }
        catch (OperationCanceledException)
        {
            Document.ErrorMessage = "Exécution annulée.";
        }
        catch (Exception ex)
        {
            Document.ErrorMessage = ex.Message;
        }
        finally
        {
            IsExecuting = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        OnPropertyChanged(nameof(Document));
    }

    private bool CanExecuteQuery() => !IsExecuting;

    [RelayCommand]
    private void CancelQuery()
    {
        _cancellationTokenSource?.Cancel();
    }
}