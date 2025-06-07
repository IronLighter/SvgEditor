using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using sample4.Controls;
using System;
using static sample4.Controls.InstrumentPanel;

namespace sample4.Shapes
{
    public class Selection
    {
        private Shape? _selectedArea;
        public int ZIndex { get; set; }
        private ShapeType _shapeType;


        private readonly SolidColorBrush? _mainColor = InstrumentPanel.SelectedMainColor;
        private readonly SolidColorBrush? _borderColor = InstrumentPanel.SelectedBorderColor;
        private readonly double _thickness = InstrumentPanel.SelectedBorderThickness;

        public Selection(ShapeType shapeType)
        {
            _shapeType = shapeType;
            CreateSelection();
        }

        private void CreateSelection()
        {
            _selectedArea = _shapeType switch
            {
                ShapeType.Rectangle or ShapeType.Square => new Rectangle(),
                ShapeType.Ellipse or ShapeType.Circle => new Ellipse(),
                ShapeType.Line => new Line(),
                _ => new Rectangle()
            };
            if(_mainColor != null)
            {
                _selectedArea.Fill = new SolidColorBrush(_mainColor.Color, 0.5);
            }
            if (_borderColor != null)
            {
                _selectedArea.Stroke = new SolidColorBrush(_borderColor.Color, 0.5);
            }
            _selectedArea.StrokeThickness = _thickness;
            _selectedArea.StrokeDashArray = [5, 3];
            _selectedArea.ZIndex = ZIndex;
        }

        public void Show()
        {
            if (_selectedArea != null)
            {
                _selectedArea.IsVisible = true;
            }
        }
        public void Hide()
        {
            if (_selectedArea != null)
            {
                _selectedArea.IsVisible = false;
                _selectedArea = null;
            }
        }

        public void Draw(Canvas canvas)
        {
            if (_selectedArea != null)
            {
                canvas.Children.Add(_selectedArea);
            }
        }
        public void Update(Point startPos, Point currentPos)
        {
            if (_selectedArea != null)
            {
                var Width = Math.Abs(startPos.X - currentPos.X);
                var Height = Math.Abs(startPos.Y - currentPos.Y);
                var Left = Math.Min(startPos.X, currentPos.X);
                var Top = Math.Min(startPos.Y, currentPos.Y);

                var Side = Math.Min(Width, Height);

                switch (_shapeType)
                {
                    case ShapeType.Rectangle:
                        _selectedArea.Width = Width;
                        _selectedArea.Height = Height;
                        Canvas.SetLeft(_selectedArea, Left);
                        Canvas.SetTop(_selectedArea, Top);
                        break;
                    case ShapeType.Square:
                        _selectedArea.Width = Side;
                        _selectedArea.Height = Side;
                        Canvas.SetLeft(_selectedArea, startPos.X < currentPos.X ? startPos.X : startPos.X - Side);
                        Canvas.SetTop(_selectedArea, startPos.Y < currentPos.Y ? startPos.Y : startPos.Y - Side);
                        break;
                    case ShapeType.Ellipse:
                        _selectedArea.Width = Width;
                        _selectedArea.Height = Height;
                        Canvas.SetLeft(_selectedArea, Left);
                        Canvas.SetTop(_selectedArea, Top);
                        break;
                    case ShapeType.Circle:
                        _selectedArea.Width = Side;
                        _selectedArea.Height = Side;
                        Canvas.SetLeft(_selectedArea, startPos.X < currentPos.X ? startPos.X : startPos.X - Side);
                        Canvas.SetTop(_selectedArea, startPos.Y < currentPos.Y ? startPos.Y : startPos.Y - Side);
                        break;
                    case ShapeType.Line:
                        if (_selectedArea is Line line)
                        {
                            line.StartPoint = startPos;
                            line.EndPoint = currentPos;
                        }
                        break;
                }
            }
        }
    }
}
