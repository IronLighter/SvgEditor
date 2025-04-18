using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using System;
using System.Collections.Generic;

namespace sample4.Controls.Shapes;

public partial class MyPoint : UserControl
{
    public Point Position { get; set; } //координаты точки
    public Action? UpdateElement;

    public List<Line> ConnectedLines = [];
    public double Radius = 10;
    private bool _dragging; //выделение
    public MyPoint()
    {
        PointerPressed += MyPoint_PointerPressed;
        PointerMoved += MyPoint_PointerMoved;
        PointerReleased += MyPoint_PointerReleased;

        InitializeComponent();
    }

    private void MyPoint_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _dragging = true;
        e.Handled = true;
    }
    private void MyPoint_PointerMoved(object? sender, PointerEventArgs e)
    {
        var mousePos = e.GetPosition(this); //позици€ курсора
        if (_dragging)
        {
            var deltaX = mousePos.X - Position.X;
            var deltaY = mousePos.Y - Position.Y;

            Position += new Point(Canvas.GetLeft(this) + deltaX, Canvas.GetTop(this) + deltaY);

            Canvas.SetLeft(this, Position.X);
            Canvas.SetTop(this, Position.Y);

            UpdateElement?.Invoke();
        }
        else
        {
            Position = mousePos;
        }
    }
    private void MyPoint_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _dragging = false;
    }
}