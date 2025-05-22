using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System;

namespace sample4.Controls.Shapes;

public partial class MyPoint : UserControl
{
    private bool _dragging;
    public double Radius = 10; //радиус публичный, чтобы линии могли его учитывать по координатам 
    public Point Position
    {
        get => new(Canvas.GetLeft(this), Canvas.GetTop(this));
        set
        {
            Canvas.SetLeft(this, value.X);
            Canvas.SetTop(this, value.Y);
        }
    }
    public Action? UpdateElement;

    public MyPoint()
    {
        InitializeComponent();
        SetupEvents();
    }
    private void SetupEvents()
    {
        PointerPressed += MyPoint_PointerPressed;
        PointerMoved += MyPoint_PointerMoved;
        PointerReleased += MyPoint_PointerReleased;
    }

    private void MyPoint_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _dragging = true;
        e.Handled = true;
    }
    private void MyPoint_PointerMoved(object? sender, PointerEventArgs e)
    {
        var mousePos = e.GetPosition(this);
        if (_dragging)
        {
            var deltaX = mousePos.X - Position.X;
            var deltaY = mousePos.Y - Position.Y;

            Position += new Point(Canvas.GetLeft(this) + deltaX, Canvas.GetTop(this) + deltaY);

            UpdateElement?.Invoke();
            e.Handled = true;
        }
    }
    private void MyPoint_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _dragging = false;
        e.Handled = true;
    }
}