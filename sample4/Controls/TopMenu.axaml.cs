using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using sample4.Views;
using System;
using System.Collections.Generic;
using System.IO;

namespace sample4.Controls;

public partial class TopMenu : UserControl
{
    public TopMenu()
    {
        InitializeComponent();
    }

    private void CreateNewButton_Click(object sender, RoutedEventArgs e)
    {
        var createNewWindow = new CreateNewWindow();
        createNewWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        if (this.VisualRoot is Window parentWindow)
        {
            createNewWindow.ShowDialog(parentWindow);
        }
    }

    [Obsolete]
    private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog();

        // Настройки диалога
        saveFileDialog.Title = "Сохранить файл";
        saveFileDialog.InitialFileName = "image.svg";
        saveFileDialog.DefaultExtension = "svg";

        // Фильтры файлов
        saveFileDialog.Filters = new List<FileDialogFilter>
    {
        new FileDialogFilter { Name = "SVG изображения", Extensions = new List<string> { "svg" } },
        new FileDialogFilter { Name = "Все файлы", Extensions = new List<string> { "*" } }
    };

        // Получаем ссылку на главное окно
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
        var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

        // Показываем диалог
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
        var result = await saveFileDialog.ShowAsync(mainWindow);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

        if (!string.IsNullOrEmpty(result))
        {
            // Обработка выбранного пути
            await File.WriteAllTextAsync(result, "Содержимое файла");
        }
    }

    [Obsolete]
    private async void LoadFileButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();

        // Настройки диалога
        openFileDialog.Title = "Выберите файл для загрузки";
        openFileDialog.AllowMultiple = false; // Разрешить выбор только одного файла

        // Фильтры файлов (можно указать нужные форматы)
        openFileDialog.Filters = new List<FileDialogFilter>
    {
        new FileDialogFilter { Name = "SVG изображения", Extensions = new List<string> { "svg" } },
        new FileDialogFilter { Name = "Все файлы", Extensions = new List<string> { "*" } }
    };

        // Получаем ссылку на главное окно
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
        var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

        // Показываем диалог и получаем результат
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
        var result = await openFileDialog.ShowAsync(mainWindow);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

        // Если пользователь выбрал файл (result - это массив путей)
        if (result != null && result.Length > 0)
        {
            string selectedFilePath = result[0]; // Получаем первый выбранный файл

            try
            {
                // Проверяем расширение файла
                string fileExtension = Path.GetExtension(selectedFilePath).ToLower();

                if (fileExtension != ".svg")
                {
                    // Диалог с предупреждением о неверном формате
                    var warningDialog = new Window
                    {
                        Title = "Неверный формат",
                        Content = new TextBlock { Text = "Пожалуйста, выберите файл с расширением .svg", Margin = new Thickness(20) },
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    await warningDialog.ShowDialog(mainWindow);
                    return;
                }

                // Ваша обработка SVG файла
                // ...

                // Уведомление об успехе
                var successDialog = new Window
                {
                    Title = "Успешно",
                    Content = new TextBlock { Text = $"SVG файл загружен: {Path.GetFileName(selectedFilePath)}", Margin = new Thickness(20) },
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                await successDialog.ShowDialog(mainWindow);
            }
            catch (Exception ex)
            {
                // Диалог с ошибкой
                var errorDialog = new Window
                {
                    Title = "Ошибка",
                    Content = new TextBlock { Text = $"Ошибка при загрузке SVG файла: {ex.Message}", Margin = new Thickness(20) },
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                await errorDialog.ShowDialog(mainWindow);
            }
        }
    }
}