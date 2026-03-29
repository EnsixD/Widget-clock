using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WidgetClock.Models;
using WidgetClock.Services;
using WidgetClock.ViewModels;

namespace WidgetClock.Views;

public partial class SettingsWindow : Window
{
    private readonly ClockSettings _settings;
    private readonly ClockSettings _originalSettings;
    private readonly ClockViewModel _viewModel;
    private readonly SettingsService _settingsService;
    private bool _isLoading = true;

    private static readonly string[] PopularFonts =
    {
        "Segoe UI Light", "Segoe UI", "Segoe UI Semibold",
        "Arial", "Arial Narrow", "Calibri", "Calibri Light",
        "Cambria", "Candara", "Century Gothic",
        "Consolas", "Courier New",
        "Franklin Gothic Medium",
        "Georgia", "Impact",
        "Lucida Console", "Lucida Sans Unicode",
        "Microsoft Sans Serif",
        "Palatino Linotype",
        "Tahoma", "Times New Roman", "Trebuchet MS",
        "Verdana",
        "Yu Gothic UI", "Yu Gothic UI Light",
    };

    public SettingsWindow(ClockSettings settings, ClockViewModel viewModel, SettingsService settingsService)
    {
        InitializeComponent();
        _settings = settings;
        _viewModel = viewModel;
        _settingsService = settingsService;
        _originalSettings = CloneSettings(settings);

        LoadFonts();
        LoadValues();
        _isLoading = false;
    }

    private void LoadFonts()
    {
        FontFamilyCombo.Items.Clear();
        foreach (var font in PopularFonts)
            FontFamilyCombo.Items.Add(font);
    }

    private void LoadValues()
    {
        _isLoading = true;

        Radio24h.IsChecked = _settings.Use24HourFormat;
        Radio12h.IsChecked = !_settings.Use24HourFormat;
        ShowSecondsCheck.IsChecked = _settings.ShowSeconds;
        ShowDateCheck.IsChecked = _settings.ShowDate;
        DateFormatBox.Text = _settings.DateFormat;

        FontFamilyCombo.SelectedItem = _settings.FontFamily;
        if (FontFamilyCombo.SelectedItem == null)
        {
            FontFamilyCombo.Items.Add(_settings.FontFamily);
            FontFamilyCombo.SelectedItem = _settings.FontFamily;
        }
        FontSizeSlider.Value = _settings.FontSize;
        DateFontSizeSlider.Value = _settings.DateFontSize;
        FontSizeLabel.Text = $"{(int)_settings.FontSize}";
        DateFontSizeLabel.Text = $"{(int)_settings.DateFontSize}";

        TimeColorBox.Text = _settings.TimeColor;
        DateColorBox.Text = _settings.DateColor;
        BgColorBox.Text = _settings.BackgroundColor;
        UpdateColorPreview(TimeColorPreview, _settings.TimeColor);
        UpdateColorPreview(DateColorPreview, _settings.DateColor);
        UpdateColorPreview(BgColorPreview, _settings.BackgroundColor);
        ShowBgCheck.IsChecked = _settings.ShowBackground;

        OpacitySlider.Value = _settings.WindowOpacity;
        OpacityLabel.Text = $"{(int)(_settings.WindowOpacity * 100)}%";
        CornerSlider.Value = _settings.CornerRadius;
        CornerLabel.Text = $"{(int)_settings.CornerRadius}";
        PaddingSlider.Value = _settings.WidgetPadding;
        PaddingLabel.Text = $"{(int)_settings.WidgetPadding}";
        ShowShadowCheck.IsChecked = _settings.ShowShadow;

        AlwaysOnTopCheck.IsChecked = _settings.AlwaysOnTop;
        RunAtStartupCheck.IsChecked = _settings.RunAtStartup;
        ShowInTaskbarCheck.IsChecked = _settings.ShowInTaskbar;

        UpdateDatePreview();
        _isLoading = false;
    }

    private void ApplyToViewModel()
    {
        if (_isLoading) return;
        _viewModel.ApplySettings(_settings);
    }

    // ── Time ──
    private void TimeFormat_Changed(object sender, RoutedEventArgs e)
    {
        if (_isLoading) return;
        _settings.Use24HourFormat = Radio24h.IsChecked == true;
        ApplyToViewModel();
    }

    private void Settings_Changed(object sender, RoutedEventArgs e)
    {
        if (_isLoading) return;
        _settings.ShowSeconds = ShowSecondsCheck.IsChecked == true;
        _settings.ShowDate = ShowDateCheck.IsChecked == true;
        _settings.ShowShadow = ShowShadowCheck.IsChecked == true;
        _settings.ShowBackground = ShowBgCheck.IsChecked == true;
        ApplyToViewModel();
    }

    private void System_Changed(object sender, RoutedEventArgs e)
    {
        if (_isLoading) return;
        _settings.AlwaysOnTop = AlwaysOnTopCheck.IsChecked == true;
        _settings.RunAtStartup = RunAtStartupCheck.IsChecked == true;
        _settings.ShowInTaskbar = ShowInTaskbarCheck.IsChecked == true;
        // Apply startup immediately
        StartupService.SetEnabled(_settings.RunAtStartup);
        ApplyToViewModel();
    }

    private void DateFormat_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (_isLoading) return;
        _settings.DateFormat = DateFormatBox.Text;
        UpdateDatePreview();
        ApplyToViewModel();
    }

    private void UpdateDatePreview()
    {
        try
        {
            var preview = DateTime.Now.ToString(DateFormatBox.Text, CultureInfo.CurrentCulture);
            DatePreviewLabel.Text = preview;
            DatePreviewLabel.Foreground = new SolidColorBrush(Color.FromRgb(108, 99, 255));
        }
        catch
        {
            DatePreviewLabel.Text = "Неверный формат";
            DatePreviewLabel.Foreground = new SolidColorBrush(Colors.OrangeRed);
        }
    }

    private void Font_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (_isLoading) return;
        if (FontFamilyCombo.SelectedItem is string fontName)
        {
            _settings.FontFamily = fontName;
            ApplyToViewModel();
        }
    }

    private void FontSize_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isLoading) return;
        _settings.FontSize = FontSizeSlider.Value;
        FontSizeLabel.Text = $"{(int)FontSizeSlider.Value}";
        ApplyToViewModel();
    }

    private void DateFontSize_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isLoading) return;
        _settings.DateFontSize = DateFontSizeSlider.Value;
        DateFontSizeLabel.Text = $"{(int)DateFontSizeSlider.Value}";
        ApplyToViewModel();
    }

    // ── Colors ──
    private void TimeColor_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (_isLoading) return;
        _settings.TimeColor = TimeColorBox.Text;
        UpdateColorPreview(TimeColorPreview, TimeColorBox.Text);
        ApplyToViewModel();
    }

    private void DateColor_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (_isLoading) return;
        _settings.DateColor = DateColorBox.Text;
        UpdateColorPreview(DateColorPreview, DateColorBox.Text);
        ApplyToViewModel();
    }

    private void BgColor_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (_isLoading) return;
        _settings.BackgroundColor = BgColorBox.Text;
        UpdateColorPreview(BgColorPreview, BgColorBox.Text);
        ApplyToViewModel();
    }

    private static void UpdateColorPreview(System.Windows.Controls.Border preview, string hex)
    {
        try { preview.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex)); }
        catch { preview.Background = Brushes.Transparent; }
    }

    // ── Appearance ──
    private void Opacity_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isLoading) return;
        _settings.WindowOpacity = OpacitySlider.Value;
        OpacityLabel.Text = $"{(int)(OpacitySlider.Value * 100)}%";
        ApplyToViewModel();
    }

    private void Corner_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isLoading) return;
        _settings.CornerRadius = CornerSlider.Value;
        CornerLabel.Text = $"{(int)CornerSlider.Value}";
        ApplyToViewModel();
    }

    private void Padding_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isLoading) return;
        _settings.WidgetPadding = PaddingSlider.Value;
        PaddingLabel.Text = $"{(int)PaddingSlider.Value}";
        ApplyToViewModel();
    }

    // ── Color presets ──
    private void ApplyPreset(string timeColor, string dateColor, string bgColor, bool showBg)
    {
        _settings.TimeColor = timeColor;
        _settings.DateColor = dateColor;
        _settings.BackgroundColor = bgColor;
        _settings.ShowBackground = showBg;
        _isLoading = true;
        TimeColorBox.Text = timeColor;
        DateColorBox.Text = dateColor;
        BgColorBox.Text = bgColor;
        ShowBgCheck.IsChecked = showBg;
        _isLoading = false;
        UpdateColorPreview(TimeColorPreview, timeColor);
        UpdateColorPreview(DateColorPreview, dateColor);
        UpdateColorPreview(BgColorPreview, bgColor);
        ApplyToViewModel();
    }

    private void Preset_Dark(object sender, RoutedEventArgs e)
        => ApplyPreset("#FFFFFFFF", "#BBFFFFFF", "#CC000000", true);

    private void Preset_Glass(object sender, RoutedEventArgs e)
        => ApplyPreset("#FFFFFFFF", "#CCFFFFFF", "#3300AAFF", true);

    private void Preset_Minimal(object sender, RoutedEventArgs e)
        => ApplyPreset("#FFFFFFFF", "#BBFFFFFF", "#00000000", false);

    private void Preset_White(object sender, RoutedEventArgs e)
        => ApplyPreset("#FF111111", "#CC222222", "#DDFFFFFF", true);

    // ── Footer ──
    private void Apply_Click(object sender, RoutedEventArgs e)
    {
        _settingsService.Save(_settings);
        ApplyToViewModel();
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        CopySettings(_originalSettings, _settings);
        StartupService.SetEnabled(_settings.RunAtStartup);
        ApplyToViewModel();
        Close();
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        var defaults = ClockSettings.Default();
        CopySettings(defaults, _settings);
        LoadValues();
        ApplyToViewModel();
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Cancel_Click(sender, e);

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

    // ── Clone / Copy ──
    private static ClockSettings CloneSettings(ClockSettings s) => new()
    {
        FontFamily = s.FontFamily, FontSize = s.FontSize, DateFontSize = s.DateFontSize,
        TimeColor = s.TimeColor, DateColor = s.DateColor, BackgroundColor = s.BackgroundColor,
        WindowOpacity = s.WindowOpacity, Use24HourFormat = s.Use24HourFormat,
        ShowDate = s.ShowDate, ShowSeconds = s.ShowSeconds, DateFormat = s.DateFormat,
        WindowLeft = s.WindowLeft, WindowTop = s.WindowTop, CornerRadius = s.CornerRadius,
        LockPosition = s.LockPosition, ShowShadow = s.ShowShadow, ShowBackground = s.ShowBackground,
        AlwaysOnTop = s.AlwaysOnTop, RunAtStartup = s.RunAtStartup, ShowInTaskbar = s.ShowInTaskbar,
        WidgetPadding = s.WidgetPadding,
    };

    private static void CopySettings(ClockSettings from, ClockSettings to)
    {
        to.FontFamily = from.FontFamily; to.FontSize = from.FontSize; to.DateFontSize = from.DateFontSize;
        to.TimeColor = from.TimeColor; to.DateColor = from.DateColor; to.BackgroundColor = from.BackgroundColor;
        to.WindowOpacity = from.WindowOpacity; to.Use24HourFormat = from.Use24HourFormat;
        to.ShowDate = from.ShowDate; to.ShowSeconds = from.ShowSeconds; to.DateFormat = from.DateFormat;
        to.WindowLeft = from.WindowLeft; to.WindowTop = from.WindowTop; to.CornerRadius = from.CornerRadius;
        to.LockPosition = from.LockPosition; to.ShowShadow = from.ShowShadow; to.ShowBackground = from.ShowBackground;
        to.AlwaysOnTop = from.AlwaysOnTop; to.RunAtStartup = from.RunAtStartup; to.ShowInTaskbar = from.ShowInTaskbar;
        to.WidgetPadding = from.WidgetPadding;
    }
}
