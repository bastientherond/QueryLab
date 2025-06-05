using System.Data.Common;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueryLab.App.Utils;
using QueryLab.App.ViewModels;
using QueryLab.App.Views;
using QueryLab.Core;

namespace QueryLab.App;

public partial class App : Application
{
    private IHost? _host;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices)
            .Build();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            var mainWindow = new MainWindow
            {
                DataContext = _host.Services.GetRequiredService<MainWindowViewModel>(),
            };

            // Maintenant que la fenêtre existe, on peut injecter la référence dans le DialogService
            var dialogService = _host.Services.GetRequiredService<IDialogService>() as DialogService;
            var storageService = _host.Services.GetRequiredService<IStorageService>() as StorageService;
            dialogService?.SetMainWindow(mainWindow);
            storageService?.SetMainWindow(mainWindow);

            desktop.MainWindow = mainWindow;
        }


        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Tu déclares ici tous tes ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<IQueryExecutor, QueryExecutor>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IStorageService, StorageService>();

        // C'est ici que tu brancheras plus tard ton Core, tes Providers, etc.
        DbProviderFactories.RegisterFactory("Npgsql", Npgsql.NpgsqlFactory.Instance);
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}