using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using sample4.Controls;
using System;
using System.Collections;
using System.Collections.Generic;

namespace sample4.Shapes
{
    public class MyRect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color MainColor { get; set; }
        public Color BorderColor { get; set; }
        public double BorderThickness { get; set; }

        private readonly int _pointsCount = 4;
        private readonly List<MyPoint> _points = [];
        private readonly List<Line> _lines = [];
        private Polygon _rectanglePolygon = new();
        private Path _filling = new();
        private bool _isSquare;
        private bool _isSelected = false;

        private Point _startPoint;
        private Point _endPoint;

        private readonly SolidColorBrush? _mainColor = InstrumentPanel.SelectedMainColor;
        private readonly SolidColorBrush? _borderColor = InstrumentPanel.SelectedBorderColor;
        private readonly double _thickness = InstrumentPanel.SelectedBorderThickness;

        public MyRect(Point start, Point end, bool isSquare)
        {
            _isSquare = isSquare;
            _startPoint = start;
            _endPoint = end;

            CreatePoints(_startPoint, _endPoint);
            //CreateLines();
            CreatePolygon();
            //CreateFilling();
        }

        private void CreatePoints(Point start, Point end)
        {
            if (_isSquare)
            {
                double side = Width;
                end = new Point(start.X + (end.X > start.X ? side : -side),
                               start.Y + (end.Y > start.Y ? side : -side));
            }


            var cords = new[]
            {
                start,                      //top left
                new Point(end.X, start.Y), //top right
                end,                       //bottom right
                new Point(start.X, end.Y) //bottom left
            };

            for (int i = 0; i < _pointsCount; i++)
            {
                var point = new MyPoint();
                point.Position = new(cords[i].X - point.Radius, cords[i].Y - point.Radius);
                point.UpdateElement += UpdateRect;
                _points.Add(point);
            }
        }

        private void CreatePolygon()
        {
            _rectanglePolygon = new Polygon
            {
                Stroke = _borderColor,
                StrokeThickness = _thickness,
                Fill = _mainColor,
                Points = new Points() // Инициализируем коллекцию точек
            };

            UpdatePolygon();
        }
        private void UpdatePolygon()
        {
            var points = new Points();

            // Добавляем все точки прямоугольника по порядку
            for (int i = 0; i < _pointsCount; i++)
            {
                points.Add(new Point(
                    _points[i].Position.X + _points[i].Radius,
                    _points[i].Position.Y + _points[i].Radius));
            }

            // Для замкнутой фигуры добавляем первую точку в конец
            if (_pointsCount > 0)
            {
                points.Add(new Point(
                    _points[0].Position.X + _points[0].Radius,
                    _points[0].Position.Y + _points[0].Radius));
            }

            _rectanglePolygon.Points = points;
        }

        private void CreateLines()
        {
            for (int i = 0; i < _pointsCount; i++)
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
            for (int i = 0; i < _pointsCount; i++)
            {
                var nextIndex = (i + 1) % _pointsCount;
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

        public void Draw(Canvas canvas)
        {
            /*canvas.Children.Add(_filling);

            foreach(var line in _lines)
            {
                canvas.Children.Add(line);
            }*/

            canvas.Children.Add(_rectanglePolygon);

            if (_isSelected)
            {
                foreach (var point in _points)
                {
                    canvas.Children.Add(point);
                }
            }

            AddCode();
        }
        private void UpdateRect()
        {
            //UpdateFilling();
            //UpdateLines();
            UpdatePolygon();
        }

        private void UpdateAttributes()
        {
            X = (int)Math.Min(_startPoint.X, _endPoint.X);
            Y = (int)Math.Min(_startPoint.Y, _endPoint.Y);
            if (!_isSquare)
            {
                Width = (int)Math.Abs(_startPoint.X - _endPoint.X);
                Height = (int)Math.Abs(_startPoint.Y - _endPoint.Y);
            }
            else
            {
                var Side = Math.Min(Width, Height);
                Width = Side;
                Height = Side;
            }
            if (_mainColor != null)
            {
                MainColor = _mainColor.Color;
            }
            if (_borderColor != null)
            {
                BorderColor = _borderColor.Color;
            }
            BorderThickness = Math.Round(_thickness, 2);
        }
        private void AddCode()
        {
            UpdateAttributes();
            CodePanel.MyCodePanel.Text += $"\n\t<rect " +
                    $"x=\"{X}\" y=\"{Y}\" width=\"{Width}\" height=\"{Height}\" " +
                    $"fill=\"#{ColorToHexConverter.ToHexString(MainColor, AlphaComponentPosition.Trailing)}\" " +
                    $"stroke=\"#{ColorToHexConverter.ToHexString(BorderColor, AlphaComponentPosition.Trailing)}\" " +
                    $"stroke-width=\"{BorderThickness}\"/>";
        }
    }
}
