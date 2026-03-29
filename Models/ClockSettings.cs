namespace WidgetClock.Models;

public class ClockSettings
{
    // Time
    public string FontFamily { get; set; } = "Segoe UI Light";
    public double FontSize { get; set; } = 72;
    public double DateFontSize { get; set; } = 18;
    public string TimeColor { get; set; } = "#FFFFFFFF";
    public string DateColor { get; set; } = "#BBFFFFFF";
    public bool Use24HourFormat { get; set; } = true;
    public bool ShowDate { get; set; } = true;
    public bool ShowSeconds { get; set; } = false;
    public string DateFormat { get; set; } = "dddd, d MMMM";

    // Background
    public string BackgroundColor { get; set; } = "#66000000";
    public bool ShowBackground { get; set; } = true;
    public bool ShowShadow { get; set; } = true;
    public double CornerRadius { get; set; } = 20;

    // Window
    public double WindowOpacity { get; set; } = 1.0;
    public double WindowLeft { get; set; } = 100;
    public double WindowTop { get; set; } = 100;
    public bool LockPosition { get; set; } = false;

    // Window padding
    public double WidgetPadding { get; set; } = 28;

    // System
    public bool RunAtStartup { get; set; } = false;
    public bool AlwaysOnTop { get; set; } = true;
    public bool ShowInTaskbar { get; set; } = false;

    public static ClockSettings Default() => new ClockSettings();
}
