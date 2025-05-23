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
    public static SolidColorBrush SelectedMainColor { get; private set; } = new(Colors.White);
    public static SolidColorBrush SelectedBorderColor { get; private set; } = new(Colors.Black);
    public static double SelectedBorderThickness { get; private set; } = 1.5;
    public static ShapeType SelectedShapeType { get; private set; } = ShapeType.Rectangle;


    private byte _red = 255;
    private byte _green = 255;
    private byte _blue = 255;
    private byte _borderRed = 0;
    private byte _borderGreen = 0;
    private byte _borderBlue = 0;

    public enum ShapeType
    {
        Rectangle,
        Line,
    }
    public InstrumentPanel()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        var stackPanel = new StackPanel
        {
            Spacing = 5,
            Margin = new Thickness(5),
            Width = 200
        };

        stackPanel.Children.Add(new TextBlock
        {
            Text = "Instruments",
            FontSize = 16,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        });

        stackPanel.Children.Add(CreateShapeButtonsPanel());

        stackPanel.Children.Add(CreateColorPalette("Main color", color =>
        {
            SelectedMainColor = new SolidColorBrush(color);
        }, _red, _green, _blue, SelectedMainColor));

        stackPanel.Children.Add(CreateColorPalette("Border color", color =>
        {
            SelectedBorderColor = new SolidColorBrush(color);
        }, _borderRed, _borderGreen, _borderBlue, SelectedBorderColor));

        stackPanel.Children.Add(CreateBorderThicknessControl());

        Content = stackPanel;
        
    }

    private WrapPanel CreateShapeButtonsPanel()
    {
        var panel = new WrapPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        };

        var shapes = new Dictionary<string, ShapeType>
            {
                { "Rect", ShapeType.Rectangle },
                { "Line", ShapeType.Line }
            };

        foreach (var shape in shapes)
        {
            var button = new Button
            {
                Content = shape.Key,
                Tag = shape.Value,
            };

            button.Click += (s, e) =>
            {
                SelectedShapeType = (ShapeType)button.Tag;
                UpdateButtonsVisual(panel, button);
            };

            panel.Children.Add(button);
        }

        return panel;
    }

    private StackPanel CreateColorPalette(string title,Action<Color> updateAction,byte r, byte g, byte b,IBrush currentColor)
    {
        var panel = new StackPanel { Spacing = 8, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center };

        panel.Children.Add(new TextBlock
        {
            Text = title,
            FontWeight = FontWeight.SemiBold
        });

        var colorPreview = new Border
        {
            Height = 30,
            Background = currentColor,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(1)
        };
        panel.Children.Add(colorPreview);

        panel.Children.Add(CreateColorSlider("R", r, val =>
        {
            r = (byte)val;
            updateAction(Color.FromRgb(r, g, b));
            colorPreview.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }));

        panel.Children.Add(CreateColorSlider("G", g, val =>
        {
            g = (byte)val;
            updateAction(Color.FromRgb(r, g, b));
            colorPreview.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }));

        panel.Children.Add(CreateColorSlider("B", b, val =>
        {
            b = (byte)val;
            updateAction(Color.FromRgb(r, g, b));
            colorPreview.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }));

        return panel;
    }


    private StackPanel CreateColorSlider(string label, byte initialValue, Action<int> onValueChanged)
    {
        var panel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 5
        };

        panel.Children.Add(new TextBlock
        {
            Text = label,
            Width = 20
        });

        var slider = new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = initialValue,
            Width = 120
        };

        var valueBox = new TextBlock
        {
            Text = initialValue.ToString(),
            Width = 30
        };

        slider.PropertyChanged += (s, e) =>
        {
            if (e.Property == RangeBase.ValueProperty)
            {
                var value = (int)slider.Value;
                valueBox.Text = value.ToString();
                onValueChanged(value);
            }
        };

        panel.Children.Add(slider);
        panel.Children.Add(valueBox);

        return panel;
    }


    private StackPanel CreateBorderThicknessControl()
    {
        var panel = new StackPanel { Spacing = 5, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center };

        panel.Children.Add(new TextBlock
        {
            Text = "Thickness",
        });

        var slider = new Slider
        {
            Minimum = 0.5,
            Maximum = 5,
            Value = SelectedBorderThickness,
            Width = 120
        };

        slider.PropertyChanged += (s, e) =>
        {
            if (e.Property == Slider.ValueProperty)
            {
                SelectedBorderThickness = slider.Value;
            }
        };

        panel.Children.Add(slider);
        return panel;
    }

    private void UpdateButtonsVisual(Panel container, Button selectedButton)
    {
        foreach (var child in container.Children)
        {
            if (child is Button button)
            {
                button.BorderBrush = button == selectedButton
                    ? Brushes.Blue
                    : Brushes.Gray;
                button.BorderThickness = button == selectedButton
                    ? new Thickness(2)
                    : new Thickness(1);
            }
        }
    }
}