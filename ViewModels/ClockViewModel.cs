using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WidgetClock.Models;

namespace WidgetClock.ViewModels;

public class ClockViewModel : ViewModelBase
{
    private readonly DispatcherTimer _timer;
    private ClockSettings _settings;

    private string _timeText = "";
    private string _dateText = "";
    private Visibility _dateVisibility = Visibility.Visible;
    private FontFamily _fontFamily = new("Segoe UI Light");
    private double _fontSize = 72;
    private double _dateFontSize = 18;
    private SolidColorBrush _timeBrush = Brushes.White;
    private SolidColorBrush _dateBrush = Brushes.White;
    private SolidColorBrush _backgroundBrush = new(Color.FromArgb(102, 0, 0, 0));
    private double _windowOpacity = 1.0;
    private double _cornerRadius = 20;
    private bool _showShadow = true;
    private bool _alwaysOnTop = true;
    private double _widgetPadding = 28;

    public string TimeText { get => _timeText; private set => SetProperty(ref _timeText, value); }
    public string DateText { get => _dateText; private set => SetProperty(ref _dateText, value); }
    public Visibility DateVisibility { get => _dateVisibility; private set => SetProperty(ref _dateVisibility, value); }
    public FontFamily FontFamily { get => _fontFamily; set => SetProperty(ref _fontFamily, value); }
    public double FontSize { get => _fontSize; set => SetProperty(ref _fontSize, value); }
    public double DateFontSize { get => _dateFontSize; set => SetProperty(ref _dateFontSize, value); }
    public SolidColorBrush TimeBrush { get => _timeBrush; set => SetProperty(ref _timeBrush, value); }
    public SolidColorBrush DateBrush { get => _dateBrush; set => SetProperty(ref _dateBrush, value); }
    public SolidColorBrush BackgroundBrush { get => _backgroundBrush; set => SetProperty(ref _backgroundBrush, value); }
    public double WindowOpacity { get => _windowOpacity; set => SetProperty(ref _windowOpacity, value); }
    public double CornerRadius { get => _cornerRadius; set => SetProperty(ref _cornerRadius, value); }
    public bool ShowShadow { get => _showShadow; set => SetProperty(ref _showShadow, value); }
    public bool AlwaysOnTop { get => _alwaysOnTop; set => SetProperty(ref _alwaysOnTop, value); }
    public double WidgetPadding { get => _widgetPadding; set => SetProperty(ref _widgetPadding, value); }

    public ClockViewModel(ClockSettings settings)
    {
        _settings = settings;
        ApplySettings(settings);

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        _timer.Tick += (_, _) => UpdateTime();
        _timer.Start();
        UpdateTime();
    }

    public void ApplySettings(ClockSettings settings)
    {
        _settings = settings;
        FontFamily = new FontFamily(settings.FontFamily);
        FontSize = settings.FontSize;
        DateFontSize = settings.DateFontSize;
        TimeBrush = ParseBrush(settings.TimeColor, "#FFFFFFFF");
        DateBrush = ParseBrush(settings.DateColor, "#BBFFFFFF");

        if (settings.ShowBackground)
            BackgroundBrush = ParseBrush(settings.BackgroundColor, "#66000000");
        else
            BackgroundBrush = new SolidColorBrush(Colors.Transparent);

        WindowOpacity = settings.WindowOpacity;
        CornerRadius = settings.CornerRadius;
        ShowShadow = settings.ShowShadow && settings.ShowBackground;
        AlwaysOnTop = settings.AlwaysOnTop;
        WidgetPadding = settings.WidgetPadding;
        DateVisibility = settings.ShowDate ? Visibility.Visible : Visibility.Collapsed;
        UpdateTime();
    }

    private void UpdateTime()
    {
        var now = DateTime.Now;
        string timeFormat;
        if (_settings.Use24HourFormat)
            timeFormat = _settings.ShowSeconds ? "HH:mm:ss" : "HH:mm";
        else
            timeFormat = _settings.ShowSeconds ? "hh:mm:ss tt" : "hh:mm tt";

        TimeText = now.ToString(timeFormat);

        if (_settings.ShowDate)
        {
            try { DateText = now.ToString(_settings.DateFormat, CultureInfo.CurrentCulture); }
            catch { DateText = now.ToString("dddd, d MMMM"); }
            DateVisibility = Visibility.Visible;
        }
        else
        {
            DateVisibility = Visibility.Collapsed;
        }
    }

    private static SolidColorBrush ParseBrush(string hex, string fallback)
    {
        try
        {
            var color = (Color)ColorConverter.ConvertFromString(hex);
            return new SolidColorBrush(color);
        }
        catch
        {
            var color = (Color)ColorConverter.ConvertFromString(fallback);
            return new SolidColorBrush(color);
        }
    }

    public void Stop() => _timer.Stop();
}
