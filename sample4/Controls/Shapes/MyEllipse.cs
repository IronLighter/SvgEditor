using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace sample4.Controls.Shapes
{
    public class MyEllipse
    {
        private readonly List<MyPoint> _points = [];
        private Ellipse _ellipse;

        private readonly SolidColorBrush? _mainColor = InstrumentPanel.SelectedMainColor;
        private readonly SolidColorBrush? _borderColor = InstrumentPanel.SelectedBorderColor;
        private readonly double _thickness = InstrumentPanel.SelectedBorderThickness;

        public MyEllipse(Point start, Point end, bool isCircle)
        {
            _ellipse = new Ellipse()
            {
                Fill = _mainColor,
                Stroke = _borderColor,
                StrokeThickness = _thickness,
            };
            CreateEllipse(start, end, isCircle);
        }

        private void CreateEllipse(Point start, Point end, bool isCircle)
        {
            if (isCircle)
            {
                double side = Math.Min(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
                end = new Point(start.X + (end.X > start.X ? side : -side),
                               start.Y + (end.Y > start.Y ? side : -side));
            }

            _ellipse.Width = Math.Abs(end.X - start.X);
            _ellipse.Height = Math.Abs(end.Y - start.Y);

            double left = Math.Min(start.X, end.X);
            double top = Math.Min(start.Y, end.Y);
            Canvas.SetLeft(_ellipse, left);
            Canvas.SetTop(_ellipse, top);

        }

        private void CreatePoints(Point start, Point end, bool isCircle)
        {
            var cords = new[]
            {
                start,                      //top left
                new Point(end.X, start.Y), //top right
                end,                       //bottom right
                new Point(start.X, end.Y) //bottom left
            };


            for (int i = 0; i < 4; i++)
            {
                var point = new MyPoint();
                point.Position = new(cords[i].X - point.Radius, cords[i].Y - point.Radius);
                point.UpdateElement += () => UpdateEllipse();
                _points.Add(point);
            }
        }

        public void DrawEllipse(Canvas canvas)
        {
            canvas.Children.Add(_ellipse);
        }
        private void UpdateEllipse()
        {
        }
    }
}
