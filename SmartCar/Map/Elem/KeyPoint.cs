using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class KeyPoint {
        // 关键点ID
        public int id {get; set;}
        // 位置信息（x,y,w）
        public double x { get; set;}
        public double y { get; set; }
        public double w { get; set; }
        // 关键点类型（）
        public int type { get; set; }
        // 校准信息
        public double Can_Adj { get; set; }
        public double UrgK { get; set;}  //拟合出小车前方直线的斜率（单位：度）
        public double UrgB { get; set;}  //拟合出小车前方直线的截距（单位：mm）

        // 通道行进模式
        public double wayType { get; set; }

        // 距通道口距离
        public double disWay { get; set; }
        // 是否返回
        public bool moveBack { get; set; }

        public KeyPoint() { }
        public KeyPoint(KeyPoint p) {
            this.x = p.x;
            this.y = p.y;
            this.w = p.w;
        }
        /// <summary>
        /// Get the distance between two points
        /// </summary>
        public double getDis(KeyPoint point)
        {
            return this.getDis(point.x, point.y);
        }
        /// <summary>
        /// Get the distance between two points
        /// </summary>
        public double getDis(double x, double y)
        {
            double dx = this.x - x;
            double dy = this.y - y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public double dd = 0.0003;
        public bool comparePos(KeyPoint p)
        {
            double px = this.x - p.x;
            double py = this.y - p.y;
            return (Math.Sqrt(px * px + py * py) < dd);
            //return this.x == p.x && this.y == p.y && this.w == p.w;
        }

    }
}
