using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using sample4.Controls;
using System;
using System.Collections.Generic;


namespace sample4.Shapes
{
    public class MyLine
    {
        public int X1 { get; set; } //start point x
        public int Y1 { get; set; } //start point y
        public int X2 { get; set; } //end point x
        public int Y2 { get; set; } //end point y
        public Color BorderColor { get; set; }
        public double BorderThickness { get; set; }

        private readonly int _pointsCount = 2;
        private readonly List<MyPoint> _points = [];
        private Line _line = new();
        private bool _isSelected = false;
        private Point _startPoint;
        private Point _endPoint;

        private readonly SolidColorBrush? _borderColor = InstrumentPanel.SelectedBorderColor;
        private readonly double _thickness = InstrumentPanel.SelectedBorderThickness;

        public MyLine(Point start, Point end)
        {
            _startPoint = start;
            _endPoint = end;

            CreatePoints(start, end);
            CreateLine();
        }

        private void CreatePoints(Point start, Point end)
        {
            var cords = new[]
            {
                start,
                end
            };

            for (int i = 0; i < _pointsCount; i++)
            {
                var point = new MyPoint();
                point.Position = new(cords[i].X - point.Radius, cords[i].Y - point.Radius);
                point.UpdateElement += () => UpdateLine();
                _points.Add(point);
            }
        }

        private void CreateLine()
        {
            _line = new()
            {
                Stroke = _borderColor,
                StrokeThickness = _thickness
            };
            UpdateLine();
        }
        private void UpdateLine()
        {
            _line.StartPoint = new(_points[0].Position.X + _points[0].Radius, _points[0].Position.Y + _points[0].Radius);
            _line.EndPoint = new(_points[1].Position.X + _points[1].Radius, _points[1].Position.Y + _points[1].Radius);
        }

        public void Draw(Canvas canvas)
        {
            canvas.Children.Add(_line);

            if (_isSelected)
            {
                foreach (var point in _points)
                {
                    canvas.Children.Add(point);
                }
            }

            AddCode();
        }

        private void UpdateAttributes()
        {
            X1 = (int)_startPoint.X;
            Y1 = (int)_startPoint.Y;
            X2 = (int)_endPoint.X;
            Y2 = (int)_endPoint.Y;
            if (_borderColor != null)
            {
                BorderColor = _borderColor.Color;
            }
            BorderThickness = Math.Round(_thickness, 2);
        }
        private void AddCode()
        {
            UpdateAttributes();
            CodePanel.MyCodePanel.Text += $"\n\t<line " +
                    $"x1=\"{X1}\" y1=\"{Y1}\" x2=\"{X2}\" y2=\"{Y2}\" " +
                    $"stroke=\"#{ColorToHexConverter.ToHexString(BorderColor, AlphaComponentPosition.Trailing)}\" " +
                    $"stroke-width=\"{BorderThickness}\"/>";
        }
    }
}
