using System;
using System.Windows;
using System.Windows.Media;

namespace LineSample
{
    public enum LineEndWidth
    {
        Small,
        Medium,
        Large
    }

    public enum LineEndLength
    {
        Small,
        Medium,
        Large
    }

    public static class LineShapeHelper
    {
        public static Geometry GetGeometryByArrowType(string arrowType,double strokeThickness, LineEndWidth lineEndWidth, LineEndLength lineEndLength, Point firstPoint, Point lastPoint)
        {
            var arrowWidth = GetArrowWidth(arrowType, lineEndWidth, strokeThickness);
            var arrowHeight = GetArrowLength(arrowType, lineEndLength, strokeThickness);
            Geometry geometry = arrowType switch
            {
                "Triangle" => GetTriangleGeometry(firstPoint, lastPoint, arrowWidth, arrowHeight),
                "Arrow" => GetArrowGeometry(firstPoint, lastPoint, arrowWidth, arrowHeight),
                "Diamond" => GetDiamondGeometry(firstPoint, lastPoint, arrowWidth, arrowHeight),
                "Stealth" => GetStealthGeometry(firstPoint, lastPoint, arrowWidth, arrowHeight),
                "Oval" => GetEllipseGeometry(firstPoint, lastPoint, arrowWidth, arrowHeight),
                _ => GetArrowGeometry(firstPoint, lastPoint, arrowWidth, arrowHeight),
            };
            return geometry;
        }

        private static double GetArrowWidth(string arrowType, LineEndWidth lineEndWidth,double strokeThickness)
        {
            double arrowWidth = 0;
            switch (arrowType)
            {
                case "Triangle":
                    arrowWidth = lineEndWidth switch
                    {
                        LineEndWidth.Small => 0.6 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Medium => 1.5 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Large => 3 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 1.5 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
                case "Oval":
                    arrowWidth = lineEndWidth switch
                    {
                        LineEndWidth.Small => 1 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Medium => 2 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Large => 4 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 2 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
                case "Arrow":
                    arrowWidth = lineEndWidth switch
                    {
                        LineEndWidth.Small => 3 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Medium => 4 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Large => 6 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 6 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
                case "Stealth":
                    arrowWidth = lineEndWidth switch
                    {
                        LineEndWidth.Small => 0.4 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Medium => 0.5 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Large => 1.5 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 1.5 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
                case "Diamond":
                    arrowWidth = lineEndWidth switch
                    {
                        LineEndWidth.Small => 0.2 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Medium => 0.75 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndWidth.Large => 1.5 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 0.75 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
            }
            return arrowWidth;
        }

        private static double GetArrowLength(string arrowType, LineEndLength lineEndLength,double strokeThickness)
        {
            double arrowLength = 0;
            switch (arrowType)
            {
                case "Triangle":
                    arrowLength = lineEndLength switch
                    {
                        LineEndLength.Small => 0.35* (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Medium => 0.75 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Large => 1.5 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 0.75 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
                case "Oval":
                    arrowLength = lineEndLength switch
                    {
                        LineEndLength.Small => 1 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Medium => 2 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Large => 4 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 2 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
                case "Arrow":
                    arrowLength = lineEndLength switch
                    {
                        LineEndLength.Small => 1.5 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Medium => 2 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Large => 3 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 6 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
                case "Stealth":
                    arrowLength = lineEndLength switch
                    {
                        LineEndLength.Small => 0.2 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Medium => 0.3 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Large => 0.75 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 0.75 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
                case "Diamond":
                    arrowLength = lineEndLength switch
                    {
                        LineEndLength.Small => 0.2 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Medium => 0.75 * (strokeThickness > 2 ? strokeThickness : 2),
                        LineEndLength.Large => 1.5 * (strokeThickness > 2 ? strokeThickness : 2),
                        _ => 0.75 * (strokeThickness > 2 ? strokeThickness : 2)
                    };
                    break;
            }
            return arrowLength;
        }

        private static Geometry GetTriangleGeometry(Point firstPoint, Point lastPoint, double arrowWidth, double arrowHeight)
        {
            var arrowGeometry = new StreamGeometry();
            var (beginFirst, beginSecond) = GetTrianglePoints(firstPoint, lastPoint, arrowWidth, arrowHeight);
            using var arrowContext = arrowGeometry.Open();
            arrowContext.BeginFigure(beginFirst, true, true);
            arrowContext.LineTo(lastPoint, true, false);
            arrowContext.LineTo(beginSecond, true, false);

            return arrowGeometry;
        }

        private static (Point pt1, Point pt2) GetTrianglePoints(Point first, Point last, double arrowWidth, double arrowHeight)
        {
            var theta = Math.Atan2(first.Y - last.Y, first.X - last.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            var pt1 = new Point(last.X + (arrowWidth * cos - arrowHeight * sin), last.Y + (arrowWidth * sin + arrowHeight * cos));
            var pt2 = new Point(last.X + (arrowWidth * cos + arrowHeight * sin), last.Y - (arrowHeight * cos - arrowWidth * sin));

            return (pt1, pt2);
        }

        /// <summary>
        /// 获取三角形的箭头形状
        /// </summary>
        /// <param name="firstPoint">线条开始点</param>
        /// <param name="lastPoint">线条结尾点</param>
        /// <param name="arrowWidth">箭头的宽</param>
        /// <param name="arrowHeight">箭头的高</param>
        /// <returns>Geometry</returns>
        private static Geometry GetArrowGeometry(Point firstPoint, Point lastPoint, double arrowWidth, double arrowHeight)
        {
            var arrowGeometry = new StreamGeometry();
            var (beginFirst, beginSecond) = GetArrowPoints(firstPoint, lastPoint, arrowWidth, arrowHeight);
            using var arrowContext = arrowGeometry.Open();
            arrowContext.BeginFigure(beginFirst, false, false);
            arrowContext.LineTo(lastPoint, true, false);
            arrowContext.LineTo(beginSecond, true, true);

            return arrowGeometry;
        }

        private static (Point pt1, Point pt2) GetArrowPoints(Point first, Point last, double arrowWidth, double arrowHeight)
        {
            var theta = Math.Atan2(first.Y - last.Y, first.X - last.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            var pt1 = new Point(last.X + (arrowWidth * cos - arrowHeight * sin), last.Y + (arrowWidth * sin + arrowHeight * cos));
            var pt2 = new Point(last.X + (arrowWidth * cos + arrowHeight * sin), last.Y - (arrowHeight * cos - arrowWidth * sin));

            return (pt1, pt2);
        }

        /// <summary>
        /// 获取菱形的箭头形状
        /// </summary>
        /// <param name="firstPoint">线条开始点</param>
        /// <param name="lastPoint">线条结尾点</param>
        /// <param name="arrowWidth">箭头的宽</param>
        /// <param name="arrowHeight">箭头的高</param>
        /// <returns>Geometry</returns>
        private static Geometry GetDiamondGeometry(Point firstPoint, Point lastPoint, double arrowWidth, double arrowHeight)
        {
            var arrowGeometry = new StreamGeometry();
            var (beginFirst, beginSecond, beginThrid) = GetDiamondPoints(firstPoint, lastPoint, arrowWidth, arrowHeight);
            using var arrowContext = arrowGeometry.Open();
            arrowContext.BeginFigure(beginFirst, true, true);
            arrowContext.LineTo(lastPoint, true, false);
            arrowContext.LineTo(beginSecond, true, false);
            arrowContext.LineTo(beginThrid, true, false);
            return arrowGeometry;
        }

        private static (Point pt1, Point pt2, Point pt3) GetDiamondPoints(Point first, Point last, double arrowWidth, double arrowHeight)
        {
            var theta = Math.Atan2(first.Y - last.Y, first.X - last.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            var pt1 = new Point(last.X + (arrowWidth * cos - arrowHeight * sin), last.Y + (arrowWidth * sin + arrowHeight * cos));
            var pt2 = new Point(last.X + (arrowWidth * cos + arrowHeight * sin), last.Y - (arrowHeight * cos - arrowWidth * sin));

            var pt3 = new Point(last.X + (arrowWidth * 2 * cos), last.Y + (arrowWidth * 2 * sin));
            return (pt1, pt2, pt3);
        }

        /// <summary>
        /// 获取尾燕箭头形状
        /// </summary>
        /// <param name="firstPoint">线条开始点</param>
        /// <param name="lastPoint">线条结尾点</param>
        /// <param name="arrowWidth">箭头的宽</param>
        /// <param name="arrowHeight">箭头的高</param>
        /// <returns>Geometry</returns>
        private static Geometry GetStealthGeometry(Point firstPoint, Point lastPoint, double arrowWidth, double arrowHeight)
        {
            var arrowGeometry = new StreamGeometry();
            var (beginFirst, beginSecond, beginThrid) = GetStealthPoints(firstPoint, lastPoint, arrowWidth, arrowHeight);
            using var arrowContext = arrowGeometry.Open();
            arrowContext.BeginFigure(beginFirst, true, true);
            arrowContext.LineTo(lastPoint, true, false);
            arrowContext.LineTo(beginSecond, true, false);
            arrowContext.LineTo(beginThrid, true, false);

            return arrowGeometry;
        }

        private static (Point pt1, Point pt2, Point pt3) GetStealthPoints(Point first, Point last, double arrowWidth, double arrowHeight)
        {
            var theta = Math.Atan2(first.Y - last.Y, first.X - last.X);
            var sin = Math.Sin(theta);
            var cos = Math.Cos(theta);
            var pt1 = new Point(last.X + (arrowWidth * cos - arrowHeight * sin), last.Y + (arrowWidth * sin + arrowHeight * cos));
            var pt2 = new Point(last.X + (arrowWidth * cos + arrowHeight * sin), last.Y - (arrowHeight * cos - arrowWidth * sin));

            var pt3 = new Point(last.X + (arrowWidth / 1.4 * cos), last.Y + (arrowWidth / 1.4 * sin));
            return (pt1, pt2, pt3);
        }

        /// <summary>
        /// 获取椭圆箭头形状
        /// </summary>
        /// <param name="firstPoint">线条开始点</param>
        /// <param name="lastPoint">线条结尾点</param>
        /// <param name="arrowWidth">箭头的宽</param>
        /// <param name="arrowHeight">箭头的高</param>
        /// <returns>Geometry</returns>
        internal static Geometry GetEllipseGeometry(Point firstPoint, Point lastPoint, double arrowWidth, double arrowHeight)
        {
            var theta = Math.Atan2(firstPoint.Y - lastPoint.Y, firstPoint.X - lastPoint.X);
            var angle = theta / Math.PI * 180;

            //椭圆的长短半轴
            var wR = arrowWidth / 2;
            var hR = arrowHeight / 2;

            var arrowGeometry = new StreamGeometry();
            using var arrowContext = arrowGeometry.Open();
            arrowContext.BeginFigure(lastPoint, true, true);

            var cicle = Math.PI * 2;
            var stAng = cicle / 2;
            var swAng = cicle / 4;
            var (endPoint, isLargeArcFlag, isClockwise) = GetArcToPoint(lastPoint, wR, hR, stAng, swAng);
            arrowContext.ArcTo(endPoint, new Size(wR, hR), 0, isLargeArcFlag, isClockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, false);

            stAng = 3 * cicle / 4;
            swAng = cicle / 4;
            (endPoint, isLargeArcFlag, isClockwise) = GetArcToPoint(endPoint, wR, hR, stAng, swAng);
            arrowContext.ArcTo(endPoint, new Size(wR, hR), 0, isLargeArcFlag, isClockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, false);

            stAng = 0;
            swAng = cicle / 4;
            (endPoint, isLargeArcFlag, isClockwise) = GetArcToPoint(endPoint, wR, hR, stAng, swAng);
            arrowContext.ArcTo(endPoint, new Size(wR, hR), 0, isLargeArcFlag, isClockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, false);

            stAng = cicle / 4;
            swAng = cicle / 4;
            (endPoint, isLargeArcFlag, isClockwise) = GetArcToPoint(endPoint, wR, hR, stAng, swAng);
            arrowContext.ArcTo(endPoint, new Size(wR, hR), 0, isLargeArcFlag, isClockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, false);

            arrowGeometry.Transform = new RotateTransform(angle, endPoint.X, endPoint.Y);
            return arrowGeometry;
        }

        /// <summary>
        /// 获取弧线的终点坐标
        /// </summary>
        /// <param name="currentPoint">当前坐标</param>
        /// <param name="wR">椭圆长半轴</param>
        /// <param name="hR">椭圆短半轴</param>
        /// <param name="stAng">开始角</param>
        /// <param name="swAng">摆动角</param>
        /// <returns></returns>
        private static (Point endPoint, bool isLargeArcFlag, bool isClockwise) GetArcToPoint(Point currentPoint, double wR, double hR, double stAng, double swAng)
        {
            var p1 = GetEllipsePoint(wR, hR, stAng);
            var p2 = GetEllipsePoint(wR, hR, stAng + swAng);
            var pt = new Point(currentPoint.X - p1.X + p2.X,
                currentPoint.Y - p1.Y + p2.Y);

            var isLargeArcFlag = Math.Abs(swAng) >= Math.PI;
            var isClockwise = swAng > 0;
            return (pt, isLargeArcFlag, isClockwise);
        }

        /// <summary>
        /// 获取椭圆任意一点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        private static Point GetEllipsePoint(double a, double b, double theta)
        {
            var aSinTheta = a * System.Math.Sin(theta);
            var bCosTheta = b * System.Math.Cos(theta);
            var circleRadius = System.Math.Sqrt((aSinTheta * aSinTheta) + (bCosTheta * bCosTheta));
            return new Point(a * bCosTheta / circleRadius, b * aSinTheta / circleRadius);
        }
    }
}
