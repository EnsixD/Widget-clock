# WidgetClock

Минималистичный виджет часов для рабочего стола Windows, построенный на WPF / .NET 10.

Прозрачное окно без рамки, всегда поверх других окон — показывает время и дату с полной визуальной настройкой.

---

## Возможности

- Отображение времени в форматах **12ч / 24ч**, опционально с секундами
- Отображение даты с настраиваемым форматом (например, `dddd, d MMMM`)
- Выбор шрифта, размера времени и даты
- Цвета времени, даты и фона в формате `#AARRGGBB`
- Встроенные цветовые пресеты: **Тёмный**, **Стекло**, **Без фона**, **Светлый**
- Регулировка прозрачности, скругления углов и внутренних отступов
- Тень под виджетом
- Перетаскивание мышью, блокировка позиции
- Запуск вместе с Windows (автостарт через реестр)
- Всегда поверх всех окон / показ в панели задач — настраивается
- Настройки сохраняются в `%AppData%\WidgetClock\settings.json`

---

## Скриншоты

> *(добавьте скриншоты в папку `assets/` и обновите пути ниже)*

---

## Требования

- Windows 10 x64 или новее
- [.NET 10 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) — для запуска из исходников
  *(опубликованный exe содержит всё необходимое, отдельная установка не нужна)*

---

## Запуск

### Готовый exe (рекомендуется)

Скачайте `WidgetClock.exe` из раздела [Releases](../../releases) и запустите.
Установка не требуется. Настройки хранятся в `%AppData%\WidgetClock\settings.json`.

### Сборка из исходников

```bash
git clone https://github.com/your-username/WidgetClock.git
cd WidgetClock
dotnet build
```

Запуск:

```bash
dotnet run
```

Публикация в единый self-contained exe:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

---

## Использование

- **Правая кнопка мыши** на виджете — открыть меню (настройки, выход, блокировка позиции)
- **Перетаскивание** — переместить виджет в любое место экрана
- В окне **Настроек** — три колонки: Время/Шрифт, Цвета, Внешний вид/Система

---

## Структура проекта

```
WidgetClock/
├── Models/
│   └── ClockSettings.cs       # Все настройки (шрифт, цвета, позиция и т.д.)
├── Services/
│   ├── SettingsService.cs     # Загрузка/сохранение JSON
│   └── StartupService.cs      # Автозапуск через реестр Windows
├── ViewModels/
│   ├── ClockViewModel.cs      # Логика часов, DispatcherTimer
│   └── ViewModelBase.cs       # INotifyPropertyChanged
├── Views/
│   ├── MainWindow.xaml(.cs)   # Прозрачное окно виджета
│   └── SettingsWindow.xaml(.cs) # Панель настроек
├── Resources/
│   └── Styles.xaml            # Тёмная тема (кнопки, слайдеры, переключатели)
└── Converters/                # BoolToVisibility, DoubleToPercent, DoubleToInt
```

---

## Технологии

| | |
|---|---|
| Язык | C# 13 |
| UI | WPF (Windows Presentation Foundation) |
| Фреймворк | .NET 10 |
| Персистентность | JSON (`System.Text.Json`) |
| Архитектура | MVVM |

---

## Лицензия

[MIT](LICENSE)
