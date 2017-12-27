using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SmartCar {
    public class DrawFormat {
        // 一米代表多少像素
        public int Rate { get; set; }
        // 设置或获取图像宽度（像素）
        public int Width { get; set; }
        // 设置或获取图像高度（像素）
        public int Height { get; set; }
        // 地图边界
        public double Padd { get; set; }
        // X = 0的点实际坐标
        public double Xadd { get; set; }
        // Y = 0的点实际坐标
        public double Yadd { get; set; }
        // 点的大小
        public int PointSize { get; set; }
        // 路径画笔
        public Pen LinePen { get; set; }

        /// <summary>
        /// 画图格式的构造函数
        /// </summary>
        /// <param name="rate">米占比像素</param>
        /// <param name="pointSize">关键点大小</param>
        /// <param name="padd">地图边界</param>
        /// <param name="xadd">X=0实际坐标</param>
        /// <param name="yadd">Y=0实际坐标</param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        public DrawFormat(int rate = 60, int pointSize = 10,
                          double padd = 1, double xadd = 1, double yadd = 1,
                          int width = 1000, int height = 1000) {
            this.Rate = rate;
            this.PointSize = pointSize;
            this.LinePen = new Pen(new SolidBrush(Color.SkyBlue), 3) { CustomEndCap = new AdjustableArrowCap(3, 3, true) };
            this.Padd = padd;
            this.Xadd = xadd;
            this.Yadd = yadd;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// 根据地图模型自动调整画布大小
        /// </summary>
        /// <param name="map">当前地图模型</param>
        public void selfAdjustSize(MapModel map) {
            // 初始化最大最小值
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            // 更新最大最小值
            for (int i = 0; i < map.Points.Count; ++i) {
                KeyPoint p = map.Points[i];
                minX = Math.Min(p.x, minX);
                maxX = Math.Max(p.x, maxX);
                minY = Math.Min(p.y, minY);
                maxY = Math.Max(p.y, maxY);
            }
            minX = (minX == double.MaxValue) ? 0 : minX;
            maxX = (maxX == double.MinValue) ? 0 : maxX;
            minY = (minY == double.MaxValue) ? 0 : minY;
            maxY = (maxY == double.MinValue) ? 0 : maxY;
            // 设置图像相关设置
            this.Xadd = this.Padd - minX;
            this.Yadd = this.Padd - minY;
            this.Width = (int)((maxX - minX + 2 * this.Padd) * Rate);
            this.Height = (int)((maxY - minY + 2 * this.Padd) * Rate);
        }

        /// <summary>
        /// 获取当前点在画布上实际位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point getDrawPoint(double x, double y) {
            return new Point((int)((Xadd + x) * Rate),
                             this.Height - (int)((Yadd + y) * Rate));
        }

    }
}
