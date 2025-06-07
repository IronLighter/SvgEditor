using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace sample4.Views;

public partial class CreateNewWindow : Window
{
    public CreateNewWindow()
    {
        InitializeComponent();
    }
    private void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        // Логика сохранения
        CloseWindow();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        CloseWindow();
    }

    private void CloseWindow()
    {
        if (this.VisualRoot is Window window)
        {
            window.Close();
        }
    }
}