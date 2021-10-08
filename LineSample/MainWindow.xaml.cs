using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using static LineSample.LineShapeHelper;

namespace LineSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Path = "M 0.000,0.000 L 311.331,206";

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

        private List<string> LineEndWidths = new List<string>()
        {
              "Small",
              "Medium",
              "Large",
        };

        private List<string> LineEndLengths = new List<string>()
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
            HeadEndWidth.ItemsSource = LineEndWidths;
            HeadEndLength.ItemsSource = LineEndLengths;
            HeadEndCombox.SelectedIndex = 1;
            HeadEndWidth.SelectedIndex = 1;
            HeadEndLength.SelectedIndex = 1;


            TailEndCombox.ItemsSource = LineEndType;
            TailEndWidth.ItemsSource = LineEndWidths;
            TailEndLength.ItemsSource = LineEndLengths;
            TailEndCombox.SelectedIndex = 0;
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
                    if (Enum.TryParse<LineEndWidth>(HeadEndWidth.SelectedValue.ToString(), out var headEndWidth)
                        && Enum.TryParse<LineEndLength>(HeadEndLength.SelectedValue.ToString(), out var headEndLenth))
                    {
                        DrawGeometry(headEndType, headEndWidth, headEndLenth, last, first, drawingContext);
                    }
                }
                var tailEndType = TailEndCombox.SelectedValue?.ToString();
                if (tailEndType != null)
                {
                    if (Enum.TryParse<LineEndWidth>(HeadEndWidth.SelectedValue.ToString(), out var headEndWidth) &&
                        Enum.TryParse<LineEndLength>(HeadEndLength.SelectedValue.ToString(), out var headEndLenth))
                    {
                        DrawGeometry(tailEndType, headEndWidth, headEndLenth, first, last, drawingContext);
                    }
                }
            }
            this.PathGrid.Background = new VisualBrush(drawingVisual) { Stretch = Stretch.None };
        }

        private void DrawGeometry(string arrowType, LineEndWidth lineEndWidth, LineEndLength lineEndLength, Point firstPoint, Point lastPoint, DrawingContext drawingContext)
        {
            var arrowGeometry = GetGeometryByArrowType(arrowType, StrokeThickness, lineEndWidth, lineEndLength, firstPoint, lastPoint);
            drawingContext.DrawGeometry(Stroke, Pen, arrowGeometry);
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
    }
}
