using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections.Generic;

namespace sample4.Controls.Shapes
{
    public class MyRect
    {
        private readonly double _cornerCount = 4;

        private readonly Canvas _canvas;
        private readonly List<MyPoint> _points = [];
        private readonly List<Line> _lines = [];
        private Path _filling = new();
        private readonly List<Point> _cords;

        public MyRect(Canvas canvas, double width, double height, double left, double top)
        {
            _canvas = canvas;
            _cords =
            [
                new(left, top), // Верхний левый
                new(left + width, top), // Верхний правый
                new(left + width, top + height), // Нижний правый
                new(left, top + height) // Нижний левый
            ];
        }

        private readonly SolidColorBrush _mainColor = new(Colors.LightBlue);
        private readonly SolidColorBrush _borderColor = new(Colors.Black);
        private readonly double _thickness = 1.5;

        private void CreatePoints()
        {
            for (int i = 0; i < _cornerCount; i++)
            {
                _points.Add(new MyPoint());
                _points[i].Position = new(_cords[i].X - _points[i].Radius, _cords[i].Y - _points[i].Radius);
                _points[i].UpdateElement += () => UpdateRect();
            }
        }
        private void CreateLines()
        {
            for (int i = 0; i < _cornerCount; i++)
            {
                Line line = new Line()
                {
                    Stroke = _borderColor,
                    StrokeThickness = _thickness
                };
                _lines.Add(line);
            }
            UpdateLines();
        }
        private void UpdateLines()
        {
            for (int i = 0; i < _cornerCount; i++)
            {
                if (i == _cornerCount - 1)
                {
                    _lines[i].StartPoint = new(_points[0].Position.X + _points[0].Radius, _points[0].Position.Y + _points[0].Radius);
                    //_points[0].ConnectedLines.Add(_lines[i]);
                    _lines[i].EndPoint = new(_points[i].Position.X + _points[i].Radius, _points[i].Position.Y + _points[i].Radius);
                    //_points[i].ConnectedLines.Add(_lines[i]);
                }
                else
                {
                    _lines[i].StartPoint = new(_points[i].Position.X + _points[i].Radius, _points[i].Position.Y + _points[i].Radius);
                    //_points[i].ConnectedLines.Add(_lines[i]);
                    _lines[i].EndPoint = new(_points[i + 1].Position.X + _points[i + 1].Radius, _points[i + 1].Position.Y + _points[i + 1].Radius);
                    //_points[i + 1].ConnectedLines.Add(_lines[i]);
                }
            }
        }

        private void CreateFilling()
        {
            _filling = new Path
            {
                Fill = _mainColor,
            };
            UpdateFilling();
        }
        private void UpdateFilling()
        {
            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                var start = new Point(_points[0].Position.X + _points[0].Radius, _points[0].Position.Y + _points[0].Radius);
                context.BeginFigure(start, true);
                for (int i = 1; i < _points.Count; i++)
                {
                    var next = new Point(_points[i].Position.X + _points[i].Radius, _points[i].Position.Y + _points[i].Radius);
                    context.LineTo(next);
                }
                context.LineTo(start);
            }
            _filling.Data = geometry;
        }
        public void DrawRect()
        {
            CreatePoints();
            CreateFilling();
            CreateLines();

            _canvas.Children.Add(_filling);

            foreach (var line in _lines)
            {
                _canvas.Children.Add(line);
            }

            foreach (var point in _points)
            {
                Canvas.SetLeft(point, point.Position.X);
                Canvas.SetTop(point, point.Position.Y);
                _canvas.Children.Add(point);
            }
        }

        private void UpdateRect()
        {
            UpdateFilling();
            UpdateLines();
        }
    }
}
