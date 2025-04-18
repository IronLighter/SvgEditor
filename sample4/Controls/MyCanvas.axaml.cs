using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using sample4.Controls.Shapes;
using System;

namespace sample4.Controls;

public partial class MyCanvas : UserControl
{
    private bool _dragging; //выделение
    private Point _startPos; //позиция мыши в начале выделения
    private Point _currentPos; //текущая позиция мыши
    private readonly Rectangle _selectedArea;

    private readonly SolidColorBrush _mainColor = new(Colors.LightBlue);
    private readonly SolidColorBrush _borderColor = new(Colors.Black);
    private readonly double _thickness = 1.5;

    public MyCanvas()
    {
        PointerPressed += MyCanvas_PointerPressed;
        PointerMoved += MyCanvas_PointerMoved;
        PointerReleased += MyCanvas_PointerReleased;

        InitializeComponent();

        _selectedArea = new Rectangle()
        {
            Fill = new SolidColorBrush(_mainColor.Color, 0.5),
            Stroke = _borderColor,
            StrokeThickness = _thickness,
            StrokeDashArray = [5, 3],
            IsVisible = false
        };
        DrawingCanvas.Children.Add(_selectedArea);
    }

    private void MyCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {

        _dragging = true;
        _selectedArea.IsVisible = true;
    }
    private void MyCanvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        _currentPos = pos;
        if (_dragging)
        {
            ElementBounds selectedAreaBounds = SetElementBounds(_startPos, _currentPos);

            _selectedArea.Width = selectedAreaBounds.Width;
            _selectedArea.Height = selectedAreaBounds.Height;
            Canvas.SetLeft(_selectedArea, selectedAreaBounds.Left);
            Canvas.SetTop(_selectedArea, selectedAreaBounds.Top);
        }
        else
        {
            _startPos = pos;
        }
        //e.Handled = true;
    }
    private void MyCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_dragging == true)
        {
            _dragging = false;
            _selectedArea.ZIndex += 1;
            _selectedArea.IsVisible = false;
            AddElement();
        }
    }

    public void AddElement()
    {
        ElementBounds rectBounds = SetElementBounds(_startPos, _currentPos);
        MyRect rect = new MyRect(DrawingCanvas, rectBounds.Width, rectBounds.Height, rectBounds.Left, rectBounds.Top);
        rect.DrawRect();
    }

    public ElementBounds SetElementBounds(Point startPos, Point currentPos)
    {
        return new ElementBounds()
        {
            Width = Math.Abs(startPos.X - currentPos.X),
            Height = Math.Abs(_startPos.Y - _currentPos.Y),
            Left = Math.Min(_startPos.X, _currentPos.X),
            Top = Math.Min(_startPos.Y, _currentPos.Y)
        };
    }

    public class ElementBounds
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
    }
}