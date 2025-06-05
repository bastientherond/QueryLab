using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueryLab.App.Utils;
using QueryLab.Core;
using QueryLab.Domain;

namespace QueryLab.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IQueryExecutor _queryExecutor;
    private readonly IDialogService _dialogService;
    
    [ObservableProperty]
    private ObservableCollection<SqlEditorViewModel> _openTabs = [];

    [ObservableProperty]
    private SqlEditorViewModel? _selectedTab;
    
    public MainWindowViewModel(IQueryExecutor queryExecutor, IDialogService dialogService)
    {
        _queryExecutor = queryExecutor;
        _dialogService = dialogService;
        OpenNewTab(); 
    }
    
    [RelayCommand]
    private void OpenNewTab()
    {
        var document = new SqlEditorDocument
        {
            Title = $"New Query {OpenTabs.Count + 1}",
            Connection = new DatabaseConnectionInfo
            {
                Name = "PostgreSQL Local",
                Provider = "Npgsql",
                ConnectionString = "Host=localhost;Port=5432;Username=querylab;Password=querylab;Database=querylabdb"
            }
        };

        var editorViewModel = new SqlEditorViewModel(document, _queryExecutor, _dialogService);
        OpenTabs.Add(editorViewModel);
        SelectedTab = editorViewModel;
    }

    [RelayCommand]
    private void CloseTab(SqlEditorViewModel viewModel)
    {
        if (OpenTabs.Contains(viewModel))
            OpenTabs.Remove(viewModel);

        if (SelectedTab == viewModel)
            SelectedTab = OpenTabs.FirstOrDefault();
    }
    
    [RelayCommand]
    private static void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.Shutdown();
        }
    }
}