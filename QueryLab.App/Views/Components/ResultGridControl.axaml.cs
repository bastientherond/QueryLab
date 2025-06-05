using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using QueryLab.App.ViewModels;

namespace QueryLab.App.Views.Components;

public partial class ResultGridControl : UserControl
{
    private SqlEditorViewModel? _viewModel;
    
    public ResultGridControl()
    {
        InitializeComponent();
        this.DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.Columns.CollectionChanged -= OnColumnsChanged;
            _viewModel.Rows.CollectionChanged -= OnRowsChanged;
        }

        _viewModel = DataContext as SqlEditorViewModel;

        if (_viewModel == null) return;
        
        _viewModel.Columns.CollectionChanged += OnColumnsChanged;
        _viewModel.Rows.CollectionChanged += OnRowsChanged;
        BuildGrid(_viewModel);
    }
    
    private void OnColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_viewModel != null)
            BuildGrid(_viewModel);
    }

    private void OnRowsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_viewModel != null)
            BuildGrid(_viewModel);
    }

    private void BuildGrid(SqlEditorViewModel vm)
    {
        ResultGrid.RowDefinitions.Clear();
        ResultGrid.ColumnDefinitions.Clear();
        ResultGrid.Children.Clear();

        int columnCount = vm.Columns.Count;

        for (int i = 0; i < columnCount; i++)
        {
            ResultGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        // Header Row
        ResultGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        for (int col = 0; col < columnCount; col++)
        {
            var header = new TextBlock
            {
                Text = vm.Columns[col],
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Background = Avalonia.Media.Brushes.LightGray,
                Padding = new Thickness(5),
            };
            Grid.SetRow(header, 0);
            Grid.SetColumn(header, col);
            ResultGrid.Children.Add(header);
        }

        // Data Rows
        for (int row = 0; row < vm.Rows.Count; row++)
        {
            ResultGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            var rowData = vm.Rows[row];
            for (int col = 0; col < columnCount; col++)
            {
                var key = vm.Columns[col];
                var value = rowData[col].Value?.ToString() ?? "";

                var cell = new TextBlock
                {
                    Text = value,
                    Padding = new Thickness(5),
                };
                Grid.SetRow(cell, row + 1);
                Grid.SetColumn(cell, col);
                ResultGrid.Children.Add(cell);
            }
        }
    }
}