using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using QueryLab.App.Views;

namespace QueryLab.App.Utils;

public class DialogService : IDialogService
{
    private Window? _mainWindow;
    
    public async Task ShowErrorAsync(string message, string title = "Error")
    {
        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var textBlock = new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Thickness(20),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        var okButton = new Button
        {
            Content = "OK",
            Width = 80,
            Margin = new Thickness(0, 10, 0, 0),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        okButton.Click += (_, _) => dialog.Close();

        dialog.Content = new StackPanel
        {
            Children =
            {
                textBlock,
                okButton
            }
        };

        if (_mainWindow is null) throw new NullReferenceException("_mainWindow is null");
        await dialog.ShowDialog(_mainWindow);
    }

    public void SetMainWindow(MainWindow mainWindow) => _mainWindow =  mainWindow;
}