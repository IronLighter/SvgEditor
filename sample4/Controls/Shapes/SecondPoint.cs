using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System;

namespace sample4.Controls.Shapes;

public partial class SecondPoint : UserControl
{
    public Point Position { get; set; } //координаты точки
    public Action? UpdateElement;

    public double Radius = 10;
    private bool _dragging; //выделение
    public SecondPoint()
    {
        SetupVisual();
        SetupEvents();
    }

    private void SetupVisual()
    {
        var ellipse = new Ellipse
        {
            Width = Radius * 2,
            Height = Radius * 2,
            Fill = Brushes.White,
            Stroke = Brushes.Black,
            StrokeThickness = 1.5,
            StrokeDashArray = [4, 4]
        };

        Content = ellipse;
    }
    private void SetupEvents()
    {
        PointerPressed += SecondPoint_PointerPressed;
        PointerMoved += SecondPoint_PointerMoved;
        PointerReleased += SecondPoint_PointerReleased;
    }


    private void SecondPoint_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _dragging = true;
        e.Handled = true;
    }
    private void SecondPoint_PointerMoved(object? sender, PointerEventArgs e)
    {
        var mousePos = e.GetPosition(this); //позиция курсора
        if (_dragging)
        {
            var deltaX = mousePos.X - Position.X;
            var deltaY = mousePos.Y - Position.Y;

            Position += new Point(Canvas.GetLeft(this) + deltaX, Canvas.GetTop(this) + deltaY);

            Canvas.SetLeft(this, Position.X);
            Canvas.SetTop(this, Position.Y);

            UpdateElement?.Invoke();
            e.Handled = true;
        }
    }
    private void SecondPoint_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _dragging = false;
        e.Handled = true;
    }
}