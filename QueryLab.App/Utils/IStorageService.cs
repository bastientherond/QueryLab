using System.Threading.Tasks;
using Avalonia.Controls;

namespace QueryLab.App.Utils;

public interface IStorageService
{
    void SetMainWindow(Window? mainWindow);
    Task<string?> SaveFileAsync();
    Task<(string? Content, string? Path)> OpenFileAsync();
}
