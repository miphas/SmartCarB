using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmartCar
{
    public class DrawPolygon : DrawM
    {
        /// <summary>
        /// 存储图片以及障碍和空白区域
        /// </summary>
        private Image img;
        private Brush backColor = Brushes.LightGray;
        private Brush freeColor = Brushes.White;
        /// <summary>
        /// 保存路径信息和地图信息
        /// </summary>
        private MapModel mapModel;
        private DrawFormat format;

        /// <summary>
        /// constructor with DrawPath
        /// </summary>
        public DrawPolygon(MapModel mapModel, DrawFormat format)
        {
            // save path image and mapmodel
            this.mapModel = mapModel;
            this.format = format;
            // try to draw background of path here
            img = new Bitmap(format.Width, format.Height);
            this.draw(mapModel);
        }

        /// <summary>
        /// 执行画图的函数
        /// </summary>
        /// <param name="map">地图模型</param>
        public override void draw(MapModel map) {
            // 获取画笔
            Graphics g = Graphics.FromImage(img);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.FillRectangle(backColor, 0, 0, img.Width, img.Height);
            // 画背景的每个部分
            foreach (Segment seg in map.Segments) {
                // 将左右侧的点拼接起来
                List<Point> ps = new List<Point>();
                foreach (var lp in seg.LeftP) {
                    ps.Add(format.getDrawPoint(lp.x, lp.y));
                }
                for (int i = seg.RightP.Count - 1; i >= 0; --i) {
                    ps.Add(format.getDrawPoint(seg.RightP[i].x, seg.RightP[i].y));
                }
                // 点的数量不为0时才画图
                if (ps.Count != 0) {
                    g.FillPolygon(freeColor, ps.ToArray());
                }
            }
        }

        /// <summary>
        /// 返回所绘背景图
        /// </summary>
        public override Image getDrawImg() {
            return img;
        }

        
    }
}
