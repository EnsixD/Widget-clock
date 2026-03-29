# WidgetClock

A minimalist desktop clock widget for Windows built with WPF / .NET 10.

Transparent, borderless, always-on-top window displaying time and date with full visual customization.

---

## Features

- **12h / 24h** time format, optionally with seconds
- Date display with a customizable format (e.g. `dddd, d MMMM`)
- Font family and size for time and date independently
- Colors for time, date, and background in `#AARRGGBB` format
- Built-in color presets: **Dark**, **Glass**, **No Background**, **Light**
- Adjustable opacity, corner radius, and inner padding
- Drop shadow
- Drag to reposition, position lock
- Run at Windows startup (via registry)
- Always-on-top and taskbar visibility — configurable
- Settings persisted to `%AppData%\WidgetClock\settings.json`

---

## Requirements

- Windows 10 x64 or later
- [.NET 10 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) — only needed when building from source
  *(the published exe is self-contained, no separate install required)*

---

## Getting Started

### Pre-built exe (recommended)

Download `WidgetClock.exe` from [Releases](../../releases) and run it. No installation needed.

### Build from source

```bash
git clone https://github.com/EnsixD/Widget-clock.git
cd Widget-clock
dotnet build
```

Run:

```bash
dotnet run
```

Publish as a single self-contained exe:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

---

## Usage

- **Right-click** the widget to open the context menu (Settings, Lock Position, Exit)
- **Drag** to move the widget anywhere on screen
- The **Settings** window has three columns: Time & Font, Colors, Appearance & System

---

## Project Structure

```
WidgetClock/
├── Models/
│   └── ClockSettings.cs          # All settings as a POCO (font, colors, position, etc.)
├── Services/
│   ├── SettingsService.cs        # JSON load/save
│   └── StartupService.cs         # Windows registry autostart
├── ViewModels/
│   ├── ClockViewModel.cs         # Clock logic, DispatcherTimer
│   └── ViewModelBase.cs          # INotifyPropertyChanged
├── Views/
│   ├── MainWindow.xaml(.cs)      # Transparent borderless widget window
│   └── SettingsWindow.xaml(.cs)  # Settings panel
├── Resources/
│   └── Styles.xaml               # Dark theme (buttons, sliders, toggles, combos)
└── Converters/                   # BoolToVisibility, DoubleToPercent, DoubleToInt
```

---

## Tech Stack

| | |
|---|---|
| Language | C# 13 |
| UI | WPF (Windows Presentation Foundation) |
| Framework | .NET 10 |
| Persistence | JSON (`System.Text.Json`) |
| Pattern | MVVM |

---

## License

[MIT](LICENSE)
