using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace LineSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Path = "M 0.000,0.000 L 311.331,106";

        private readonly Dictionary<Point, List<Point>> _arrowPointsDictionary = new Dictionary<Point, List<Point>>();

        protected double ArrowWidth { get; set; }
        protected double ArrowHeight { get; set; }

        private Pen Pen { get; set; }

        private Brush Stroke { get; set; } = new SolidColorBrush(Color.FromRgb(47, 82, 143));

        private Brush Fill { get; } = new SolidColorBrush(Color.FromRgb(68, 114, 196));

        private List<string> LineEndType = new List<string>()
        {
              "None",
              "Triangle",
              "Stealth",
              "Diamond",
              "Oval",
              "Arrow",
        };

        private List<string> LineEndWidth = new List<string>()
        {
              "Small",
              "Medium",
              "Large",
        };

        private List<string> LineEndLength = new List<string>()
        {
              "Small",
              "Medium",
              "Large",
        };

        private double StrokeThickness { get; } = 14;

        public MainWindow()
        {
            InitializeComponent();
            HeadEndCombox.ItemsSource = LineEndType;
            HeadEndWidth.ItemsSource = LineEndWidth;
            HeadEndLength.ItemsSource = LineEndLength;
            HeadEndCombox.SelectedIndex = 1;
            HeadEndWidth.SelectedIndex = 1;
            HeadEndLength.SelectedIndex = 1;


            TailEndCombox.ItemsSource = LineEndType;
            TailEndWidth.ItemsSource = LineEndWidth;
            TailEndLength.ItemsSource = LineEndLength;
            TailEndCombox.SelectedIndex = 3;
            TailEndWidth.SelectedIndex = 1;
            TailEndLength.SelectedIndex = 1;
        }

        private void OnRender(string path)
        {
            Point first;
            Point last;
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                var geometry = Geometry.Parse(path);
                first = geometry.Bounds.TopLeft;
                last = geometry.Bounds.BottomRight;
                Pen = new Pen();
                RefreshPen(Pen);
                drawingContext.DrawGeometry(Fill, Pen, geometry);
                var headEndType = HeadEndCombox.SelectedValue?.ToString();
                if (headEndType != null)
                {
                    ArrowWidth = GetArrowWidth(HeadEndWidth.SelectedValue.ToString(), headEndType);
                    ArrowHeight = GetArrowLength(HeadEndLength.SelectedValue.ToString(), headEndType);
                    DrawGeometry(headEndType, last, first, drawingContext);
                }
                var tailEndType = TailEndCombox.SelectedValue?.ToString();
                if (tailEndType != null)
                {
                    ArrowWidth = GetArrowWidth(TailEndWidth.SelectedValue.ToString(), tailEndType);
                    ArrowHeight = GetArrowLength(TailEndLength.SelectedValue.ToString(), tailEndType);
                    DrawGeometry(tailEndType, first, last, drawingContext);
                }
            }
            this.PathGrid.Background = new VisualBrush(drawingVisual) { Stretch = Stretch.None };
        }

        private void DrawGeometry(string arrowType, Point firstPoint, Point lastPoint, DrawingContext drawingContext)
        {
            var isFill = arrowType != "Arrow";
            _arrowPointsDictionary.Clear();
            var arrowGeometry = GetGeometryByArrowType(arrowType, firstPoint, lastPoint);
            drawingContext.DrawGeometry(Stroke, Pen, arrowGeometry);
        }

        private Geometry GetGeometryByArrowType(string arrowType, Point firstPoint, Point lastPoint)
        {
            Geometry geometry = arrowType switch
            {
                "Triangle" => GetTriangleGeometry(firstPoint, lastPoint),
                "Arrow" => GetArrowGeometry(firstPoint, lastPoint),
                "Diamond" => GetDiamondGeometry(firstPoint, lastPoint),
                "Stealth" => GetStealthGeometry(firstPoint, lastPoint),
                "Oval" => GetEllipseGeometry(firstPoint, lastPoint),
                _ => GetTriangleGeometry(firstPoint, lastPoint),
            };
            return geometry;
        }

        private Geometry GetTriangleGeometry(Point firstPoint, Point lastPoint)
        {
            var arrowGeometry = new StreamGeometry();
            Point beginFirst = new Point(), beginSecond = new Point();
            GetTrianglePoints(firstPoint, lastPoint, ref beginFirst, ref beginSecond);
            _arrowPointsDictionary.Add(lastPoint, new List<Point> { beginFirst, beginSecond }); //前端的箭头
            using var arrowContext = arrowGeometry.Open();
            foreach (KeyValuePair<Point, List<Point>> keyValuePair in _arrowPointsDictionary)
            {
                if (keyValuePair.Value == null) continue;
                arrowContext.BeginFigure(keyValuePair.Value[0], true, true);
                arrowContext.LineTo(keyValuePair.Key, true, false);
                arrowContext.LineTo(keyValuePair.Value[1], true, false);
            }
            return arrowGeometry;
        }

        private Geometry GetStealthGeometry(Point firstPoint, Point lastPoint)
        {
            var arrowGeometry = new StreamGeometry();
            var (beginFirst, beginSecond, beginThrid) = GetStealthPoints(firstPoint, lastPoint);
            _arrowPointsDictionary.Add(lastPoint, new List<Point> { beginFirst, beginSecond, beginThrid }); //前端的箭头
            using var arrowContext = arrowGeometry.Open();
            foreach (KeyValuePair<Point, List<Point>> keyValuePair in _arrowPointsDictionary)
            {
                if (keyValuePair.Value == null) continue;
                arrowContext.BeginFigure(keyValuePair.Value[0], true, true);
                arrowContext.LineTo(keyValuePair.Key, true, false);
                arrowContext.LineTo(keyValuePair.Value[1], true, false);
                arrowContext.LineTo(keyValuePair.Value[2], true, false);
            }
            return arrowGeometry;
        }

        private Geometry GetDiamondGeometry(Point firstPoint, Point lastPoint)
        {
            var arrowGeometry = new StreamGeometry();
            var (beginFirst, beginSecond, beginThrid) = GetDiamondPoints(firstPoint, lastPoint);
            _arrowPointsDictionary.Add(lastPoint, new List<Point> { beginFirst, beginSecond, beginThrid }); //前端的箭头
            using var arrowContext = arrowGeometry.Open();
            foreach (KeyValuePair<Point, List<Point>> keyValuePair in _arrowPointsDictionary)
            {
                if (keyValuePair.Value == null) continue;
                arrowContext.BeginFigure(keyValuePair.Value[0], true, true);
                arrowContext.LineTo(keyValuePair.Key, true, false);
                arrowContext.LineTo(keyValuePair.Value[1], true, false);
                arrowContext.LineTo(keyValuePair.Value[2], true, false);
            }
            return arrowGeometry;
        }

        private Geometry GetArrowGeometry(Point firstPoint, Point lastPoint)
        {
            var arrowGeometry = new StreamGeometry();
            Point beginFirst = new Point(), beginSecond = new Point();
            GetArrowPoints(firstPoint, lastPoint, ref beginFirst, ref beginSecond);
            _arrowPointsDictionary.Add(lastPoint, new List<Point> { beginFirst, beginSecond }); //前端的箭头
            using var arrowContext = arrowGeometry.Open();
            foreach (KeyValuePair<Point, List<Point>> keyValuePair in _arrowPointsDictionary)
            {
                if (keyValuePair.Value == null) continue;
                arrowContext.BeginFigure(keyValuePair.Value[0], false, false);
                arrowContext.LineTo(keyValuePair.Key, true, false);
                arrowContext.LineTo(keyValuePair.Value[1], true, true);
            }
            return arrowGeometry;
        }

        private Geometry GetEllipseGeometry(Point firstPoint, Point lastPoint)
        {
            var arrowGeometry = new StreamGeometry();
            Point beginFirst = new Point(), beginSecond = new Point();
            GetEllipsePoints(firstPoint, lastPoint, ref beginFirst, ref beginSecond);
            _arrowPointsDictionary.Add(lastPoint, new List<Point> { beginFirst, beginSecond }); //前端的箭头
            using var arrowContext = arrowGeometry.Open();
            var size = new Size(ArrowWidth / Math.PI / 4, ArrowHeight / Math.PI / 4);
            var theta = Math.Atan2(lastPoint.Y - firstPoint.Y, lastPoint.X - firstPoint.X);
            foreach (KeyValuePair<Point, List<Point>> keyValuePair in _arrowPointsDictionary)
            {
                if (keyValuePair.Value == null) continue;
                arrowContext.BeginFigure(keyValuePair.Key, true, false);
                //arrowContext.LineTo(keyValuePair.Key, true, false);
                //arrowContext.LineTo(keyValuePair.Value[1], true, false);
                arrowContext.ArcTo(keyValuePair.Value[0], size, 0, true, SweepDirection.Clockwise, true, false);
                arrowContext.ArcTo(keyValuePair.Key, size, 0, true, SweepDirection.Clockwise, true, false);
            }
            return arrowGeometry;
        }

        private double GetArrowWidth(string? lineEndWidth, string arrowType)
        {
            double arrowWidth = 0;
            switch (arrowType)
            {
                case "Triangle":
                    arrowWidth = lineEndWidth switch
                    {
                        "Small" => 0.6 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 3 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
                case "Oval":
                    arrowWidth = lineEndWidth switch
                    {
                        "Small" => 0.6 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 3 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 5 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
                case "Arrow":
                    arrowWidth = lineEndWidth switch
                    {
                        "Small" => 3 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 4 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 6 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 6 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
                case "Stealth":
                    arrowWidth = lineEndWidth switch
                    {
                        "Small" => 0.4 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 0.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
                case "Diamond":
                    arrowWidth = lineEndWidth switch
                    {
                        "Small" => 0.2 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 0.75 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 0.75 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
            }

            return arrowWidth;
        }

        private double GetArrowLength(string? lineEndLength, string arrowType)
        {
            double arrowLength = 0;
            switch (arrowType)
            {
                case "Triangle":
                    arrowLength = lineEndLength switch
                    {
                        "Small" => 0.2 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 0.75 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 0.75 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
                case "Oval":
                    arrowLength = lineEndLength switch
                    {
                        "Small" => 0.6 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 3 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 5 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
                case "Arrow":
                    arrowLength = lineEndLength switch
                    {
                        "Small" => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 2 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 3 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 6 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
                case "Stealth":
                    arrowLength = lineEndLength switch
                    {
                        "Small" => 0.17 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 0.3 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 0.75 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 0.75 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
                case "Diamond":
                    arrowLength = lineEndLength switch
                    {
                        "Small" => 0.2 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Medium" => 0.75 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        "Large" => 1.5 * (StrokeThickness > 2 ? StrokeThickness : 2),
                        _ => 0.75 * (StrokeThickness > 2 ? StrokeThickness : 2)
                    };
                    break;
            }
            return arrowLength;
        }

        /// <summary>
        /// 依据用户的设置，更新传入笔的颜色，粗细以及线型(绘制PPT多路径用到)
        /// </summary>
        private void RefreshPen(Pen pen, bool isStroke = true)
        {
            pen.Brush = isStroke ? Stroke : null;
            pen.Thickness = StrokeThickness;
            //pen.DashStyle = new System.Windows.Media.DashStyle(0, 1);
            pen.StartLineCap = PenLineCap.Flat;
            pen.EndLineCap = pen.StartLineCap;
            pen.LineJoin = PenLineJoin.Miter;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnRender(Path);
        }

        private (Point pt1, Point pt2, Point pt3) GetStealthPoints(Point first, Point last)
        {
            var theta = Math.Atan2(first.Y - last.Y, first.X - last.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            var pt1 = new Point(last.X + (ArrowWidth * cos - ArrowHeight * sin),
                 last.Y + (ArrowWidth * sin + ArrowHeight * cos));
            var pt2 = new Point(last.X + (ArrowWidth * cos + ArrowHeight * sin),
                 last.Y - (ArrowHeight * cos - ArrowWidth * sin));


            var pt3 = new Point(last.X + (ArrowWidth / 1.4 * cos), last.Y + (ArrowWidth / 1.4 * sin));
            return (pt1, pt2, pt3);
        }

        private (Point pt1, Point pt2, Point pt3) GetDiamondPoints(Point first, Point last)
        {
            var theta = Math.Atan2(first.Y - last.Y, first.X - last.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            var pt1 = new Point(last.X + (ArrowWidth * cos - ArrowHeight * sin),
                 last.Y + (ArrowWidth * sin + ArrowHeight * cos));
            var pt2 = new Point(last.X + (ArrowWidth * cos + ArrowHeight * sin),
                 last.Y - (ArrowHeight * cos - ArrowWidth * sin));


            var pt3 = new Point(last.X + (ArrowWidth * 2 * cos), last.Y + (ArrowWidth * 2 * sin));
            return (pt1, pt2, pt3);
        }

        private void GetArrowPoints(Point first, Point last, ref Point pt1, ref Point pt2)
        {
            var theta = Math.Atan2(first.Y - last.Y, first.X - last.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            pt1 = new Point(last.X + (ArrowWidth * cos - ArrowHeight * sin),
                last.Y + (ArrowWidth * sin + ArrowHeight * cos));
            pt2 = new Point(last.X + (ArrowWidth * cos + ArrowHeight * sin),
                last.Y - (ArrowHeight * cos - ArrowWidth * sin));
        }

        private void GetEllipsePoints(Point first, Point last, ref Point pt1, ref Point pt2)
        {
            var theta = Math.Atan2(last.Y - first.Y, last.X - first.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            pt1 = new Point(last.X - ArrowWidth * sin, last.Y - ArrowHeight * cos);
            pt2 = new Point(last.X - ArrowWidth * sin, last.Y - ArrowHeight * cos);
        }

        private void GetTrianglePoints(Point first, Point last, ref Point pt1, ref Point pt2)
        {
            var theta = Math.Atan2(first.Y - last.Y, first.X - last.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            pt1 = new Point(last.X + (ArrowWidth * cos - ArrowHeight * sin),
                last.Y + (ArrowWidth * sin + ArrowHeight * cos));
            pt2 = new Point(last.X + (ArrowWidth * cos + ArrowHeight * sin),
                last.Y - (ArrowHeight * cos - ArrowWidth * sin));
        }
    }
}
