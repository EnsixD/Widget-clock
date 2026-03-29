using System.Windows;
using WidgetClock.Services;
using WidgetClock.Views;

namespace WidgetClock;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var settingsService = new SettingsService();
        var settings = settingsService.Load();
        var mainWindow = new MainWindow(settings, settingsService);
        mainWindow.Show();
    }
}
