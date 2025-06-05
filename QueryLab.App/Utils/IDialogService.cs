using System.Threading.Tasks;

namespace QueryLab.App.Utils;

public interface IDialogService
{
    Task ShowErrorAsync(string message, string title = "Error");
}
