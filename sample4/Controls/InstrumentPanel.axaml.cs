using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace sample4.Controls;

public partial class InstrumentPanel : UserControl
{
    public static SolidColorBrush? SelectedMainColor { get; private set; }
    public static SolidColorBrush? SelectedBorderColor { get; private set; }
    public static double SelectedBorderThickness { get; private set; }
    public static ShapeType SelectedShapeType { get; private set; } = ShapeType.Rectangle;
    public enum ShapeType
    {
        Rectangle,
        Square,
        Ellipse,
        Circle,
        Line,
    }

    public InstrumentPanel()
    {
        InitializeComponent();

        MainColorPickerSetup();
        SelectedMainColor = new(MainColorPicker.Color);

        BorderColorPickerSetup();
        SelectedBorderColor = new(BorderColorPicker.Color);

        BorderThicknessSliderSetup();
        SelectedBorderThickness = BorderThicknessSlider.Value;
    }
    private void MainColorPickerSetup()
    {
        MainColorPicker.Color = Colors.White;
        MainColorPicker.PropertyChanged += (s, e) =>
        {
            if (e.Property == ColorPicker.ColorProperty)
            {
                SelectedMainColor = new SolidColorBrush(MainColorPicker.Color);
            }
        };
    }
    private void BorderColorPickerSetup()
    {
        BorderColorPicker.Color = Colors.Black;
        BorderColorPicker.PropertyChanged += (s, e) =>
        {
            if (e.Property == ColorPicker.ColorProperty)
            {
                SelectedBorderColor = new SolidColorBrush(BorderColorPicker.Color);
            }
        };
    }
    private void BorderThicknessSliderSetup()
    {
        BorderThicknessSlider.Value = 1.5;
        BorderThicknessSlider.PropertyChanged += (s, e) =>
        {
            if (e.Property == RangeBase.ValueProperty)
            {
                SelectedBorderThickness = BorderThicknessSlider.Value;
            }
        };
    }

    public void RectButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedShapeType = ShapeType.Rectangle;
    }
    public void SquareButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedShapeType = ShapeType.Square;
    }
    public void EllipseButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedShapeType = ShapeType.Ellipse;
    }
    public void LineButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedShapeType = ShapeType.Line;
    }
}