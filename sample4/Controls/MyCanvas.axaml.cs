using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using sample4.Controls.Shapes;
using System;
using System.Diagnostics;

namespace sample4.Controls;

public partial class MyCanvas : UserControl
{
    private bool _dragging;
    private Point _startPos; //позиция мыши в начале выделения
    private Point _currentPos; //текущая позиция мыши
    private Rectangle _selection = new();

    private readonly SolidColorBrush? _mainColor = InstrumentPanel.SelectedMainColor;
    private readonly SolidColorBrush? _borderColor = InstrumentPanel.SelectedBorderColor;
    private readonly double _thickness = InstrumentPanel.SelectedBorderThickness;

    public MyCanvas()
    {
        InitializeComponent();
        SetupEvents();

        CreateSelection();
    }

    private void CreateSelection()
    {
        _selection = new()
        {
            StrokeThickness = _thickness,
            StrokeDashArray = [5, 3],
            IsVisible = false
        };
        DrawingCanvas.Children.Add(_selection);
    }
    private void UpdateSelection()
    {
        var Width = Math.Abs(_startPos.X - _currentPos.X);
        var Height = Math.Abs(_startPos.Y - _currentPos.Y);
        var Left = Math.Min(_startPos.X, _currentPos.X);
        var Top = Math.Min(_startPos.Y, _currentPos.Y);

        _selection.Width = Width;
        _selection.Height = Height;
        _selection.Fill = _mainColor;
        _selection.Stroke = _borderColor;
        Canvas.SetLeft(_selection, Left);
        Canvas.SetTop(_selection, Top);
    }
    public void AddElement()
    {
        switch (InstrumentPanel.SelectedShapeType)
        {
            case InstrumentPanel.ShapeType.Rectangle:
                MyRect rect = new MyRect(_startPos, _currentPos, false);
                rect.DrawRect(DrawingCanvas);
                break;
            case InstrumentPanel.ShapeType.Square:
                MyRect square = new MyRect(_startPos, _currentPos, true);
                square.DrawRect(DrawingCanvas);
                break;
            case InstrumentPanel.ShapeType.Ellipse:
                MyEllipse ellipse = new MyEllipse(_startPos, _currentPos, false);
                ellipse.DrawEllipse(DrawingCanvas);
                break;
            case InstrumentPanel.ShapeType.Circle:
                MyEllipse circle = new MyEllipse(_startPos, _currentPos, true);
                circle.DrawEllipse(DrawingCanvas);
                break;
            case InstrumentPanel.ShapeType.Line:
                MyLine line = new MyLine(_startPos, _currentPos);
                line.DrawLine(DrawingCanvas);
                break;
        }
    }

    private void SetupEvents()
    {
        PointerPressed += MyCanvas_PointerPressed;
        PointerMoved += MyCanvas_PointerMoved;
        PointerReleased += MyCanvas_PointerReleased;
    }
    private void MyCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _dragging = true;
        _selection.IsVisible = true;
        e.Handled = true;
    }
    private void MyCanvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        _currentPos = e.GetPosition(this);
        if (_dragging)
        {
            UpdateSelection();
        }
        else
        {
            _startPos = _currentPos;
        }
        e.Handled = true;
    }
    private void MyCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_dragging == true)
        {
            _dragging = false;
            _selection.ZIndex += 1; //выделение переходит на слой вверх каждый раз
            _selection.IsVisible = false;
            AddElement();
        }
        e.Handled = true;
    }

}