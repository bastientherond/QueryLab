using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly IStorageService _storageService;
    
    [ObservableProperty]
    private ObservableCollection<SqlEditorViewModel> _openTabs = [];

    [ObservableProperty]
    private SqlEditorViewModel? _selectedTab;
    
    public MainWindowViewModel(IQueryExecutor queryExecutor, IDialogService dialogService,  IStorageService storageService)
    {
        _queryExecutor = queryExecutor;
        _dialogService = dialogService;
        _storageService = storageService;
    }
    
    [RelayCommand]
    private async Task OpenFileTabAsync()
    {
        var (content, path) = await _storageService.OpenFileAsync();

        if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(path))
            return;
        
        // Si un le même fichier est déjà dans l'éditeur on ne le rajoute pas 
        if(OpenTabs.Any(x => x.Document.FilePath == path))
            return;
        
        var document = new SqlEditorDocument
        {
            SqlText = content,
            Title = Path.GetFileName(path),
            FilePath = path,
            Connection = new DatabaseConnectionInfo
            {
                Name = "PostgreSQL Local",
                Provider = "Npgsql",
                ConnectionString = "Host=localhost;Port=5432;Username=querylab;Password=querylab;Database=querylabdb"
            }
        };

        var editorViewModel = new SqlEditorViewModel(document, _queryExecutor, _dialogService, _storageService);
        OpenTabs.Add(editorViewModel);
        SelectedTab = editorViewModel;
    }

    [RelayCommand]
    private void CloseTab(SqlEditorViewModel viewModel)
    {
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

    [RelayCommand]
    private void Save() => SelectedTab?.SaveCommand.Execute(null);
}