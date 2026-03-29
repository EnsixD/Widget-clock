using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WidgetClock.Models;
using WidgetClock.Services;
using WidgetClock.ViewModels;

namespace WidgetClock.Views;

public partial class MainWindow : Window
{
    private readonly ClockSettings _settings;
    private readonly SettingsService _settingsService;
    private readonly ClockViewModel _viewModel;

    public MainWindow(ClockSettings settings, SettingsService settingsService)
    {
        InitializeComponent();
        _settings = settings;
        _settingsService = settingsService;
        _viewModel = new ClockViewModel(settings);
        DataContext = _viewModel;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Left = _settings.WindowLeft;
        Top = _settings.WindowTop;
        ShowInTaskbar = _settings.ShowInTaskbar;
        Topmost = _settings.AlwaysOnTop;
        UpdateContextMenuState();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!_settings.LockPosition)
            DragMove();
    }

    private void Window_RightClick(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
        ContextPopup.IsOpen = true;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _settings.WindowLeft = Left;
        _settings.WindowTop = Top;
        _settingsService.Save(_settings);
        _viewModel.Stop();
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        ContextPopup.IsOpen = false;
        var settingsWin = new SettingsWindow(_settings, _viewModel, _settingsService);
        settingsWin.Owner = this;
        settingsWin.ShowDialog();
        // Apply window-level settings that can't bind through ViewModel
        ShowInTaskbar = _settings.ShowInTaskbar;
        Topmost = _settings.AlwaysOnTop;
        UpdateContextMenuState();
    }

    private void LockPosition_Click(object sender, RoutedEventArgs e)
    {
        _settings.LockPosition = !_settings.LockPosition;
        UpdateContextMenuState();
        ContextPopup.IsOpen = false;
    }

    private void AlwaysOnTop_Click(object sender, RoutedEventArgs e)
    {
        _settings.AlwaysOnTop = !_settings.AlwaysOnTop;
        Topmost = _settings.AlwaysOnTop;
        UpdateContextMenuState();
        ContextPopup.IsOpen = false;
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        _settings.WindowLeft = Left;
        _settings.WindowTop = Top;
        _settingsService.Save(_settings);
        Application.Current.Shutdown();
    }

    private void UpdateContextMenuState()
    {
        // Lock indicator
        LockDot.Visibility = _settings.LockPosition ? Visibility.Visible : Visibility.Collapsed;
        LockLabel.Text = _settings.LockPosition ? "Разблокировать" : "Заблокировать";
        LockIcon.Text = _settings.LockPosition ? "\uE785" : "\uE72E"; // locked/unlocked icon

        // Always on top indicator
        TopDot.Fill = _settings.AlwaysOnTop
            ? new SolidColorBrush(Color.FromRgb(108, 99, 255))
            : new SolidColorBrush(Color.FromArgb(60, 108, 99, 255));
    }

    public void RefreshViewModel()
    {
        _viewModel.ApplySettings(_settings);
    }
}
