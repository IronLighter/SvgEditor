using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections.Generic;


namespace sample4.Controls.Shapes
{
    public class MyLine
    {
        private readonly int _pointsCount = 2;
        private readonly List<MyPoint> _points = [];
        private  Line _line = new();

        private readonly SolidColorBrush? _borderColor = InstrumentPanel.SelectedBorderColor;
        private readonly double _thickness = InstrumentPanel.SelectedBorderThickness;

        public MyLine(Point start, Point end)
        {
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

        public void DrawLine(Canvas canvas)
        {
            canvas.Children.Add(_line);

            foreach (var point in _points)
            {
                canvas.Children.Add(point);
            }
        }
    }
}
