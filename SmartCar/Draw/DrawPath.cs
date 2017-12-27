using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SmartCar {
    public class DrawPath : DrawM {

        private MapModel map;
        private DrawFormat format;

        private Bitmap path;
        /// <summary>
        /// 构造路径信息
        /// </summary>
        /// <param name="map"></param>
        public DrawPath(MapModel map, DrawFormat format) {
            this.map = map;
            this.format = format;
            this.path = new Bitmap(format.Width, format.Height);
            this.draw(map);
        }

        /// <summary>
        /// 绘制路径地图
        /// </summary>
        /// <param name="map"></param>
        public override void draw(MapModel map) {
            List<Point> listP = new List<Point>();
            List<int> typeP = new List<int>();
            for (int i = 0; i < map.Points.Count; ++i) {
                KeyPoint p = map.Points[i];
                listP.Add(format.getDrawPoint(p.x, p.y));
                typeP.Add(p.type);
            }
            drawKeyPoints(listP, typeP);
            drawLines(listP, typeP);
        }

        /// <summary>
        /// 获取路径地图
        /// </summary>
        /// <returns></returns>
        public override Image getDrawImg() {
            return this.path;
        }

        /// <summary>
        /// 绘制当前位置
        /// </summary>
        /// <param name="p"></param>
        public void drawCurPos(KeyPoint pos) {
            Graphics g = Graphics.FromImage(this.path);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Point p = format.getDrawPoint(pos.x, pos.y);
            g.FillRectangle(Brushes.Red, p.X - format.PointSize / 2, p.Y - format.PointSize / 2, format.PointSize, format.PointSize);
        }

        /// <summary>
        /// 获取关键点在图上的坐标（单位：像素）
        /// </summary>
        /// <returns></returns>
        public List<Point> getPtLocInPic()
        {
            List<Point> listP = new List<Point>();
            for (int i = 0; i < map.Points.Count; ++i) {
                KeyPoint p = map.Points[i];
                listP.Add(format.getDrawPoint(p.x, p.y));
            }

            return listP;
        }

        /// <summary>
        /// 绘制关键点
        /// </summary>
        /// <param name="listP"></param>
        /// <param name="typeP"></param>
        private void drawKeyPoints(List<Point> listP, List<int> typeP) {
            // 获取画笔并平滑处理
            int half = format.PointSize / 2;
            Graphics g = Graphics.FromImage(this.path);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            for (int i = 0; i < listP.Count; ++i) {
                // 绘制普通关键点
                if (typeP[i] == (int)MapInfo.pointTypeE.General) {
                    g.FillEllipse(Brushes.Black, listP[i].X - half, listP[i].Y - half, format.PointSize, format.PointSize);
                }
                else {
                    int pz = (int)(format.PointSize * 0.6);
                    int hz = pz / 2;
                    g.FillEllipse(Brushes.Black, listP[i].X - hz, listP[i].Y - hz, pz, pz);
                    g.DrawEllipse(Pens.Black, listP[i].X - half, listP[i].Y - half, format.PointSize, format.PointSize);
                }
            }
        }
        /// <summary>
        /// 绘制带箭头的路径
        /// </summary>
        /// <param name="listP"></param>
        /// <param name="typeP"></param>
        private void drawLines(List<Point> listP, List<int> typeP) {
            // 获取画笔并平滑处理
            Graphics g = Graphics.FromImage(this.path);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int pz = format.PointSize / 2 + 1;
            for (int i = 0; i < listP.Count - 1; ++i) {
                // 起点和终点
                Point p1 = listP[i];
                Point p2 = listP[i + 1];
                double ang = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
                // 关键点本身占一定大小
                int x = (int)(Math.Cos(ang) * pz);
                int y = (int)(Math.Sin(ang) * pz);
                g.DrawLine(format.LinePen, p1.X + x, p1.Y + y, p2.X - x, p2.Y - y);
            }
        }


        
    }
}
