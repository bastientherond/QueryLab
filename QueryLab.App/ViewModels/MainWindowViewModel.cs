using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueryLab.Domain;

namespace QueryLab.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<SqlEditorViewModel> _openTabs = [];

    [ObservableProperty]
    private SqlEditorViewModel? _selectedTab;
    
    public MainWindowViewModel()
    {
        OpenNewTab();
    }
    
    [RelayCommand]
    private void OpenNewTab()
    {
        var document = new SqlEditorDocument
        {
            Title = $"New Query {OpenTabs.Count + 1}"
        };

        var editorViewModel = new SqlEditorViewModel(document);
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