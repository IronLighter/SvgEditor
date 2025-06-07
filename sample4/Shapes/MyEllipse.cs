using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using sample4.Controls;
using sample4.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace sample4.Shapes
{
    public class MyEllipse
    {
        public int CX { get; set; } //center x
        public int CY { get; set; } //center y
        public int RX { get; set; } //radius x
        public int RY { get; set; } //radius y
        public Color MainColor { get; set; }
        public Color BorderColor { get; set; }
        public double BorderThickness { get; set; }


        private readonly List<MyPoint> _points = [];
        private Ellipse _ellipse;
        private bool _isCircle;
        private Point _startPoint;
        private Point _endPoint;

        private readonly SolidColorBrush? _mainColor = InstrumentPanel.SelectedMainColor;
        private readonly SolidColorBrush? _borderColor = InstrumentPanel.SelectedBorderColor;
        private readonly double _thickness = InstrumentPanel.SelectedBorderThickness;

        public MyEllipse(Point start, Point end, bool isCircle)
        {
            _isCircle = isCircle;
            _startPoint = start;
            _endPoint = end;

            _ellipse = new Ellipse()
            {
                Fill = _mainColor,
                Stroke = _borderColor,
                StrokeThickness = _thickness,
            };
            CreateEllipse(start, end);
        }

        private void CreateEllipse(Point start, Point end)
        {
            if (_isCircle)
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
                point.UpdateElement += UpdateEllipse;
                _points.Add(point);
            }
        }

        public void Draw(Canvas canvas)
        {
            canvas.Children.Add(_ellipse);
            AddCode();
        }
        private void UpdateEllipse()
        {
        }
        private void UpdateAttributes()
        {
            var x = Math.Min(_startPoint.X, _endPoint.X);
            var y = Math.Min(_startPoint.Y, _endPoint.Y);
            var width = Math.Abs(_startPoint.X - _endPoint.X);
            var height = Math.Abs(_startPoint.Y - _endPoint.Y);



            CX = (int)(x + width / 2);
            CY = (int)(y + height / 2);
            if (!_isCircle)
            {
                RX = (int)(width / 2);
                RY = (int)(height / 2);
            }
            else
            {
                var r = (int)(Math.Min(width, height) / 2);
                RX = r;
                RY = r;
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
            if (!_isCircle)
            {
                CodePanel.MyCodePanel.Text += $"\n\t<ellipse " +
                    $"cx=\"{CX}\" cy=\"{CY}\" rx=\"{RX}\" ry=\"{RY}\" " +
                    $"fill=\"#{ColorToHexConverter.ToHexString(MainColor, AlphaComponentPosition.Trailing)}\" " +
                    $"stroke=\"#{ColorToHexConverter.ToHexString(BorderColor, AlphaComponentPosition.Trailing)}\" " +
                    $"stroke-width=\"{BorderThickness}\"/>";
            }
            else
            {
                CodePanel.MyCodePanel.Text += $"\n\t<circle " +
                    $"cx=\"{CX}\" cy=\"{CY}\" r=\"{RX}\" " +
                    $"fill=\"#{ColorToHexConverter.ToHexString(MainColor, AlphaComponentPosition.Trailing)}\" " +
                    $"stroke=\"#{ColorToHexConverter.ToHexString(BorderColor, AlphaComponentPosition.Trailing)}\" " +
                    $"stroke-width=\"{BorderThickness}\"/>";
            }
        }
    }
}
