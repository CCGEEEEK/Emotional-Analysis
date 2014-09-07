using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Collections;
 



namespace EmoAns
{
    class PieChart
    {
         private class Sector
        {
            public Point CenterPoint
            {
                get;
                set;
            }
            /// <summary>
            /// 角度制，非弧度制
            /// </summary>
            public double SectorAngle
            {
                get;
                set;
            }
            public Point ScreenEdge
            {
                get;
                set;
            }

            public Sector(Point centerpoint, double angle, Point screenEdge)
            {
                this.CenterPoint = centerpoint;
                this.SectorAngle = angle;
                this.ScreenEdge = screenEdge;
            }

            public PathGeometry SectorDrawing()
            {
                PathGeometry sectorPath = new PathGeometry();
                sectorPath.FillRule = FillRule.Nonzero;
                PathFigure sectorPathFigure = new PathFigure();
                sectorPathFigure.StartPoint = new Point(0, 0);
                sectorPathFigure.IsFilled = true;
                sectorPathFigure.IsClosed = true;
                LineSegment line1 = new LineSegment();
                line1.Point = this.CenterPoint;
                LineSegment line2 = new LineSegment();
                double r = Math.Sqrt(CenterPoint.X * CenterPoint.X + CenterPoint.Y * CenterPoint.Y);
                double x = 2 * r * Math.Sin(Angle.Angle2Rad(SectorAngle / 2)) * Math.Cos(Angle.Angle2Rad((180 - SectorAngle) / 2 - 90 + Angle.Rad2Angle(Math.Atan2(CenterPoint.X, CenterPoint.X))));
                double y = -2 * r * Math.Sin(Angle.Angle2Rad(SectorAngle / 2)) * Math.Sin(Angle.Angle2Rad((180 - SectorAngle) / 2 - 90 + Angle.Rad2Angle(Math.Atan2(CenterPoint.X, CenterPoint.X))));
                line2.Point = new Point(x, y);
                LineSegment line3 = new LineSegment();
                line3.Point = new Point(ScreenEdge.X, 0);
                sectorPathFigure.Segments.Add(line1);
                sectorPathFigure.Segments.Add(line2);
                if (SectorAngle > 180)
                {
                    LineSegment templine1 = new LineSegment();
                    templine1.Point = new Point(line2.Point.X, ScreenEdge.Y);
                    LineSegment templine2 = new LineSegment();
                    templine2.Point = ScreenEdge;
                    sectorPathFigure.Segments.Add(templine1);
                    sectorPathFigure.Segments.Add(templine2);
                }
                sectorPathFigure.Segments.Add(line3);
                sectorPath.Figures.Add(sectorPathFigure);
                return sectorPath;
            }
        }

        public Point CenterPoint
        {
            get;set;
        }
        public double Radius
        {
            get;
            set;
        }
        private double[] angles;
        private Point screenEdge;
        private Point centerPoint;
        private ChartDataItems chartDataItems;
        /// <summary>
        /// 构造饼状图
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="data">ChartDataItems类型数据集合</param>
        public PieChart(double radius, ChartDataItems data)
        {
            centerPoint = new Point(radius,radius);
            angles = new double[data.Items.Count];
            this.chartDataItems = data;
            this.Radius = radius;
            double sum = 0;
            int j = 0; ;
            foreach (ChartData i in data.Items)
            {
                sum = sum +i.Value;
            }
            foreach(ChartData i in data.Items)
            { 
                angles[j]= i.Value/ sum*360;
                j++;
            }
        }
        private EllipseGeometry circleDrawing()
        {
            EllipseGeometry eg = new EllipseGeometry();
            eg.Center = centerPoint;
            eg.RadiusX = eg.RadiusY = this.Radius;
            return eg;
        }
        /// <summary>
        /// 绘制饼形图
        /// </summary>
        /// <returns>Canvas画布</returns>
        public Canvas PieDrawing()
        {
            Canvas pieChart=new Canvas();
            pieChart.Width = this.Radius * 2 + 120;
            pieChart.Height = this.Radius * 2;
            screenEdge = new Point(pieChart.Width, pieChart.Height);
            int sectorNum = chartDataItems.Items.Count;
            double currentRotateAngle = 0;
            StackPanel legendStackPanel = new StackPanel();
            legendStackPanel.Width = 100;
            int j = 0;
            foreach (ChartData i in this.chartDataItems.Items)
            {  
                CombinedGeometry cg = new CombinedGeometry();
                cg.GeometryCombineMode = GeometryCombineMode.Intersect;
                Sector sector = new Sector(centerPoint, angles[j], screenEdge);
                cg.Geometry1 = sector.SectorDrawing();
                cg.Geometry2 = circleDrawing();
                cg.Transform = new RotateTransform(currentRotateAngle, centerPoint.X, centerPoint.Y);
                Path path = new Path();
                path.Data = cg;
                Random red = new Random((int)DateTime.Now.Ticks);
                System.Threading.Thread.Sleep(red.Next(50));
                Random blue = new Random((int)DateTime.Now.Ticks);
                System.Threading.Thread.Sleep(blue.Next(100));
                Random green = new Random((int)DateTime.Now.Ticks);
                System.Threading.Thread.Sleep(green.Next(100));
                int r=red.Next(256);
                int g = green.Next(256);
                int b = blue.Next(256);
                path.Fill = new SolidColorBrush(Color.FromRgb((byte)r,(byte)g,(byte)b));
                StackPanel legendItem = new StackPanel();
                legendItem.Orientation = Orientation.Horizontal;
                Rectangle rect = new Rectangle();
                rect.Width = rect.Height = 20;
                rect.Fill = path.Fill;
                legendItem.Children.Add(rect);
                Label legendName = new Label();
                legendName.Content = i.Name;
                legendItem.Children.Add(legendName);
                legendStackPanel.Children.Add(legendItem);
                currentRotateAngle += angles[j];
                path.ToolTip = i.Name + "\n" + (angles[j]*100/360).ToString("0.00") + "%\n" + i.Value;
                pieChart.Children.Add(path);
                j++;
            }
            
            Canvas.SetTop(legendStackPanel, this.Radius-20*j/2);
            Canvas.SetLeft(legendStackPanel, this.Radius * 2 + 20);
            pieChart.Children.Add(legendStackPanel);
            return pieChart;
        }
    }

    
}

