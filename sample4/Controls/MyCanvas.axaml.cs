using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using sample4.Shapes;
using System;

namespace sample4.Controls;

public partial class MyCanvas : UserControl
{
    private bool _dragging;
    private Point _startPos;
    private Point _currentPos;
    private Selection? _selection;

    public MyCanvas()
    {
        InitializeComponent();
        SetupEvents();
        SetCanvasSize(1000, 500);
    }
    public void SetCanvasSize(double width, double height)
    {
       
    }
    public void AddElement()
    {
        switch (InstrumentPanel.SelectedShapeType)
        {
            case InstrumentPanel.ShapeType.Rectangle:
                MyRect rect = new MyRect(_startPos, _currentPos, false);
                rect.Draw(DrawingCanvas);
                break;
            case InstrumentPanel.ShapeType.Square:
                MyRect square = new MyRect(_startPos, _currentPos, true);
                square.Draw(DrawingCanvas);
                break;
            case InstrumentPanel.ShapeType.Ellipse:
                MyEllipse ellipse = new MyEllipse(_startPos, _currentPos, false);
                ellipse.Draw(DrawingCanvas);
                break;
            case InstrumentPanel.ShapeType.Circle:
                MyEllipse circle = new MyEllipse(_startPos, _currentPos, true);
                circle.Draw(DrawingCanvas);
                break;
            case InstrumentPanel.ShapeType.Line:
                MyLine line = new MyLine(_startPos, _currentPos);
                line.Draw(DrawingCanvas);
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
        _selection = new Selection(InstrumentPanel.SelectedShapeType);
        _selection.Draw(DrawingCanvas);
        _selection.Show();
        e.Handled = true;
    }
    private void MyCanvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        _currentPos = new(Math.Round(e.GetPosition(this).X), Math.Round(e.GetPosition(this).Y));
        if (_dragging && _selection != null)
        {
            _selection.Update(_startPos, _currentPos);
        }
        else
        {
            _startPos = _currentPos;
        }
        e.Handled = true;
    }
    private void MyCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_dragging == true && _selection != null)
        {
            _dragging = false;
            _selection.ZIndex += 1; //выделение переходит на слой вверх каждый раз
            _selection.Hide();
            AddElement();
        }
        e.Handled = true;
    }
}