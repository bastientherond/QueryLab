using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Windows.Input;

namespace QueryLab.App.Behaviors;

public static class KeyGestureBehavior
{
    public static readonly AttachedProperty<KeyGesture?> GestureProperty =
        AvaloniaProperty.RegisterAttached<Control, KeyGesture?>(
            "Gesture", typeof(KeyGestureBehavior));

    public static readonly AttachedProperty<ICommand?> CommandProperty =
        AvaloniaProperty.RegisterAttached<Control, ICommand?>(
            "Command", typeof(KeyGestureBehavior));

    static KeyGestureBehavior()
    {
        GestureProperty.Changed.AddClassHandler<Control>(OnGestureChanged);
    }

    public static void SetGesture(Control control, KeyGesture? value) =>
        control.SetValue(GestureProperty, value);

    public static KeyGesture? GetGesture(Control control) =>
        control.GetValue(GestureProperty);

    public static void SetCommand(Control control, ICommand? value) =>
        control.SetValue(CommandProperty, value);

    public static ICommand? GetCommand(Control control) =>
        control.GetValue(CommandProperty);

    private static void OnGestureChanged(Control control, AvaloniaPropertyChangedEventArgs e)
    {
        control.KeyDown -= OnKeyDown;

        if (e.NewValue != null)
        {
            control.KeyDown += OnKeyDown;
        }
    }

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not Control control)
            return;

        var gesture = GetGesture(control);
        var command = GetCommand(control);

        if (gesture == null || !gesture.Matches(e) || command?.CanExecute(null) != true) return;
        command.Execute(null);
        e.Handled = true;
    }
}