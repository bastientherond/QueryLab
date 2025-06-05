using System.Reflection.Metadata;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using QueryLab.App.ViewModels;

namespace QueryLab.App.Views;

public partial class SqlEditorView : UserControl
{
    public SqlEditorView()
    {
        InitializeComponent();
    }
    
    private void OnSqlTextChanged(object sender, TextChangedEventArgs e)
    {
        if (DataContext is not SqlEditorViewModel vm) return;
        
        vm.Document.MarkDirty();
        // OnPropertyChanged(nameof(vm.Document.TabTitle));
    }

}