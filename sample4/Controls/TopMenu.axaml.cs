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

        // ��������� �������
        saveFileDialog.Title = "��������� ����";
        saveFileDialog.InitialFileName = "image.svg";
        saveFileDialog.DefaultExtension = "svg";

        // ������� ������
        saveFileDialog.Filters = new List<FileDialogFilter>
    {
        new FileDialogFilter { Name = "SVG �����������", Extensions = new List<string> { "svg" } },
        new FileDialogFilter { Name = "��� �����", Extensions = new List<string> { "*" } }
    };

        // �������� ������ �� ������� ����
#pragma warning disable CS8602 // ������������� ��������� ������ ������.
        var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
#pragma warning restore CS8602 // ������������� ��������� ������ ������.

        // ���������� ������
#pragma warning disable CS8604 // ��������, ��������-������, ����������� �������� NULL.
        var result = await saveFileDialog.ShowAsync(mainWindow);
#pragma warning restore CS8604 // ��������, ��������-������, ����������� �������� NULL.

        if (!string.IsNullOrEmpty(result))
        {
            // ��������� ���������� ����
            await File.WriteAllTextAsync(result, "���������� �����");
        }
    }

    [Obsolete]
    private async void LoadFileButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();

        // ��������� �������
        openFileDialog.Title = "�������� ���� ��� ��������";
        openFileDialog.AllowMultiple = false; // ��������� ����� ������ ������ �����

        // ������� ������ (����� ������� ������ �������)
        openFileDialog.Filters = new List<FileDialogFilter>
    {
        new FileDialogFilter { Name = "SVG �����������", Extensions = new List<string> { "svg" } },
        new FileDialogFilter { Name = "��� �����", Extensions = new List<string> { "*" } }
    };

        // �������� ������ �� ������� ����
#pragma warning disable CS8602 // ������������� ��������� ������ ������.
        var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
#pragma warning restore CS8602 // ������������� ��������� ������ ������.

        // ���������� ������ � �������� ���������
#pragma warning disable CS8604 // ��������, ��������-������, ����������� �������� NULL.
        var result = await openFileDialog.ShowAsync(mainWindow);
#pragma warning restore CS8604 // ��������, ��������-������, ����������� �������� NULL.

        // ���� ������������ ������ ���� (result - ��� ������ �����)
        if (result != null && result.Length > 0)
        {
            string selectedFilePath = result[0]; // �������� ������ ��������� ����

            try
            {
                // ��������� ���������� �����
                string fileExtension = Path.GetExtension(selectedFilePath).ToLower();

                if (fileExtension != ".svg")
                {
                    // ������ � ��������������� � �������� �������
                    var warningDialog = new Window
                    {
                        Title = "�������� ������",
                        Content = new TextBlock { Text = "����������, �������� ���� � ����������� .svg", Margin = new Thickness(20) },
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    await warningDialog.ShowDialog(mainWindow);
                    return;
                }

                // ���� ��������� SVG �����
                // ...

                // ����������� �� ������
                var successDialog = new Window
                {
                    Title = "�������",
                    Content = new TextBlock { Text = $"SVG ���� ��������: {Path.GetFileName(selectedFilePath)}", Margin = new Thickness(20) },
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                await successDialog.ShowDialog(mainWindow);
            }
            catch (Exception ex)
            {
                // ������ � �������
                var errorDialog = new Window
                {
                    Title = "������",
                    Content = new TextBlock { Text = $"������ ��� �������� SVG �����: {ex.Message}", Margin = new Thickness(20) },
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                await errorDialog.ShowDialog(mainWindow);
            }
        }
    }
}