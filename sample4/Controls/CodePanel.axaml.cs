using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sample4.Controls;

public partial class CodePanel : UserControl
{
    public static TextBox MyCodePanel {  get; set; } = new TextBox();
    public static int ImageWidth { get; set; } = 1000;
    public static int ImageHeight { get; set; } = 500;
    private string _startLine;
    private string _endLine = "\n</g></svg>";

    public CodePanel()
    {
        InitializeComponent();
        _startLine = $"<svg xmlns=\"http://www.w3.org/2000/svg\" " +
            $"viewBox=\"0 0 {ImageWidth} {ImageHeight}\" " +
            $"width=\"{ImageWidth}\" height=\"{ImageHeight}\">" +
            $"<g id=\"_01_align_center\" data-name=\"01 align center\">";

        MyCodePanel.PropertyChanged += (s, e) =>
        {
            CodeBox.Text = _startLine + MyCodePanel.Text + _endLine;
        };
    }
}