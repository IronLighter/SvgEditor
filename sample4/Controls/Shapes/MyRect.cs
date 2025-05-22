using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections;
using System.Collections.Generic;

namespace sample4.Controls.Shapes
{
    public class MyRect
    {
        private readonly int _cornerCount = 4;
        private readonly List<MyPoint> _points = [];
        private readonly List<Line> _lines = [];
        private Path _filling = new();

        private readonly SolidColorBrush _mainColor = new(Colors.LightBlue);
        private readonly SolidColorBrush _borderColor = new(Colors.Black);
        private readonly double _thickness = 1.5;

        public MyRect(Point start, Point end)
        {
            CreatePoints(start, end);
            CreateLines();
            CreateFilling();
        }

        private void CreatePoints(Point start, Point end)
        {
            var cords = new[]
            {
                start,                      //top left
                new Point(end.X, start.Y), //top right
                end,                       //bottom right
                new Point(start.X, end.Y) //bottom left
            };

            for (int i = 0; i < _cornerCount; i++)
            {
                var point = new MyPoint();
                point.Position = new(cords[i].X - point.Radius, cords[i].Y - point.Radius);
                point.UpdateElement += () => UpdateRect();
                _points.Add(point);
            }
        }

        private void CreateLines()
        {
            for (int i = 0; i < _cornerCount; i++)
            {
                Line line = new()
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
                var nextIndex = (i + 1) % _cornerCount;
                _lines[i].StartPoint = new(_points[i].Position.X + _points[i].Radius, _points[i].Position.Y + _points[i].Radius);
                _lines[i].EndPoint = new(_points[nextIndex].Position.X + _points[nextIndex].Radius, _points[nextIndex].Position.Y + _points[nextIndex].Radius);
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

        public void DrawRect(Canvas canvas)
        {
            canvas.Children.Add(_filling);

            foreach(var line in _lines)
            {
                canvas.Children.Add(line);
            }

            foreach (var point in _points)
            {
                canvas.Children.Add(point);
            }
        }
        private void UpdateRect()
        {
            UpdateFilling();
            UpdateLines();
        }
    }
}
