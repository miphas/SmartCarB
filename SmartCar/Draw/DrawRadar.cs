using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmartCar.Draw
{
    public class DrawRadar
    {
        // 保存当前图像
        private Image img;
        // 图像宽度以及高度
        private int width;
        private int height;
        private int halfWidth;
        private int halfHeight;
        // 图像缩放比例（自动计算）
        private double rate;
        // 附加放大镜比例
        private double enlarge = 3;
        // 画点的大小（PSize设置）
        private int pSize = 2;
        private int pHalfSize = 1;
        // 画点的颜色（PenStyle设置）
        private Pen penStyle = Pens.Black;
        private Color color = Color.Black;
        // 绘图起始数据点、结束数据点
        private int start = 88;//RadarINFO.filtStart;
        private int end = 640;// RadarINFO.filtEnd; 640
        // 画图的起始点和终点
        private double[] angs;
        // 设置字体
        private Font font = new Font("宋体", 9);
        private int fontSize;
        // 设置间隔
        private int interval = 20;
        private String tittle = ""; //激光雷达数据图

        private String tag = "1m";
        /// <summary>
        /// 设置或获取点的宽度大小
        /// </summary>
        public int PSize
        {
            get { return pSize; }
            set { pSize = Math.Max(1, value); pHalfSize = pSize / 2; }
        }
        /// <summary>
        /// 设置或获取画笔颜色
        /// </summary>
        public Pen PenStyle
        {
            get { return penStyle; }
            set { penStyle = value == null ? Pens.Black : null; }
        }
        /// <summary>
        /// 设置或获取放大比例
        /// </summary>
        public double Enlarge
        {
            get { return enlarge; }
            set { enlarge = Math.Max(0.1, value); }
        }
        /// <summary>
        /// 设置或获取标题
        /// </summary>
        public String Tittle
        {
            get { return tittle; }
            set { tittle = value; }
        }


        /// <summary>
        /// 构造一个雷达图像类
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public DrawRadar(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.halfWidth = width / 2;
            this.halfHeight = height / 2;
            // 长度的一半对应最远距离，防止绘出边界
            this.rate = Math.Min(width, height) / 2.0 / 5600;
            // 初始化角度数组
            this.angs = new double[end];
            for (int i = start; i < end; ++i)
            {
                angs[i] = ((i - start) / (double)510) * Math.PI - 0;
            }
        }

        public void getBaseXY(int[] data, ref int[] x, ref int[] y, ref double[] rang)
        {
            for (int i = start; i < end; i++)
            {
                rang[i] = angs[i];
                x[i] = (int)(data[i] * Math.Cos(angs[i]));
                y[i] = (int)(data[i] * Math.Sin(angs[i]));
            }
        }


        /// <summary>
        /// 绘制激光雷达数据信息图
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Image drawRadarImg(List<long> data)
        {
            // 数据不足则不进行绘制
            if (data.Count < end)
            {
                return null;
            }
            //data = new RadarZeroFilter().getFilterData(data);
            //data = new RadarKalmanFilter().getFilterData(data);
            img = new Bitmap(this.width, this.height);
            Graphics g = Graphics.FromImage(img);
            for (int i = start; i < end; ++i)
            {
                double x = data[i] * Math.Cos(angs[i]) * rate * enlarge;
                double y = data[i] * Math.Sin(angs[i]) * rate * enlarge;
                g.FillRectangle(new SolidBrush(color),
                                (int)(halfWidth + x - pHalfSize),
                                (int)(halfHeight - (y - pHalfSize)),
                                pSize, pSize);
            }
            // 一米长
            int meterLen = (int)(1000 * this.rate * enlarge);
            int posH = this.height - interval;
            int posW = this.width - interval;
            g.DrawString("1m", font, Brushes.Black, posW - g.MeasureString(tag, font).Width / 2 - meterLen / 2, posH - interval);
            g.DrawLine(penStyle, posW - meterLen, posH, posW, posH);
            // 图片标题
            g.DrawString(tittle, font, Brushes.Black, halfWidth - g.MeasureString(tittle, font).Width / 2, interval);
            return img;
        }

        /*
        public Image drawSegment(int[] data)
        {
            // 数据不足则不进行绘制
            if (data.Length < end)
            {
                return null;
            }
            img = new Bitmap(this.width, this.height);
            Graphics g = Graphics.FromImage(img);
            // 获得线段划分
            List<Map.Segment> segs = Map.Segment.getSegs(data);
            //Map.Segment.smoothData(data, segs);
            segs = Map.LocalMap.getLines(data, segs);
            Console.WriteLine(Map.LocalMap.getFeatures(segs));
            //Console.WriteLine(segs.Count);
            for (int k = 0; k < segs.Count; k++)
            {
                //Console.WriteLine(segs[k].beginIndex + " " + segs[k].endIndex);
                for (int i = segs[k].beginIndex; i < segs[k].endIndex; ++i)
                {
                    double x = data[i] * Math.Cos(angs[i]) * rate * enlarge;
                    double y = data[i] * Math.Sin(angs[i]) * rate * enlarge;
                    g.FillRectangle(new SolidBrush(k % 2 == 0 ? Color.Red : Color.Blue),
                                    (int)(halfWidth + x - pHalfSize),
                                    (int)(halfHeight - (y - pHalfSize)),
                                    pSize, pSize);
                }
            }
            return img;
        }
        */

        public Image drawOccMap(int[][] map)
        {
            Image img = new Bitmap(map[0].Length, map.Length);
            Graphics g = Graphics.FromImage(img);
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 0) continue;
                    g.DrawRectangle(map[i][j] == 1 ? Pens.Black : Pens.White, j, map.Length - i, 1, 1);
                }
            }

            return img;
        }
    }
}
