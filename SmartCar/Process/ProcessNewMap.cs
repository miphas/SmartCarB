using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SmartCar {
    public class ProcessNewMap {

        // record the segments
        private static ProcessSeg proSeg; 
        
        /// <summary>
        /// 创建一张新地图
        /// </summary>
        public static void init() {
            DataArea.mapModel = new MapModel();
        }

        /// <summary>
        /// 返回地图信息
        /// </summary>
        /// <returns></returns>
        public static MapModel getMapModel() {
            return DataArea.mapModel;
        }

        /// <summary>
        /// 记录关键点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="urgB"></param>
        /// <param name="urgK"></param>
        public static void markKeyPoint(int type, double dis = 0) {
            // 创建当前坐标点
            KeyPoint curP = PortManager.drPort.getPosition();
            curP.type = type;
            KeyPoint keyPoint = new CorrectPosition().getCurrentKB(PortManager.urgPort);
            curP.UrgB = keyPoint.UrgB;
            curP.UrgK = keyPoint.UrgK;
            curP.wayType = (type == 0) ? 0 : Form_Path.wayType;
            curP.disWay = dis;
            curP.moveBack = Form_Path.wayBack;
            // 变更地图
            lock (DataArea.mapModel) {
                // add Point 
                DataArea.mapModel.Points.Add(curP);
                // add Segment
                if (proSeg != null) {
                    if (!(DataArea.mapModel.Points.Count > 1 && curP.type == 1 &&
                        curP.getDis(DataArea.mapModel.Points[DataArea.mapModel.Points.Count - 2]) < 1.2)) {
                            DataArea.mapModel.Segments.Add(proSeg.getSegment());
                    }
                    
                }
                proSeg = new ProcessSeg();
            }
        }

        /// <summary>
        /// 移除关键点
        /// </summary>
        public static void unmarkKeyPoint() {
            lock (DataArea.mapModel) {
                if (DataArea.mapModel == null || DataArea.mapModel.Points.Count == 0) {
                    return;
                }
                DataArea.mapModel.Points.RemoveAt(DataArea.mapModel.Points.Count - 1);
            }
        }

        /// <summary>
        /// 开始刷新地图
        /// </summary>
        public static void startRefreshMap() {
            refFlag = true;
        }

        /// <summary>
        /// 结束刷新地图
        /// </summary>
        public static void stopRefreshMap() {
            refFlag = false;
        }

        /// <summary>
        /// 刷新地图
        /// </summary>

        public static bool refFlag = true;
        public static void mapping() {
            while (!Form1.closing && refFlag) {
                if (DataArea.mapModel == null || PortManager.drPort == null) {
                    Thread.Sleep(100);
                    continue;
                }
                addRadarPoint();
                showNewMap();

                Thread.Sleep(100);
            }
        }

        private static void addRadarPoint() {
            // 线段序列为空
            if (proSeg == null) {
                return;
            }
            // 数据不满足要求，则返回
            KeyPoint p = PortManager.drPort.getPosition();
            UrgModel um = PortManager.urgPort.getUrgData();
            if (um.Distance.Count < UrgInfo.ang180Index) {
                return;
            }
            // 货物激光雷达位置信息
            KeyPoint kp = CarInfo.getRadarPosition(p);
            um.Distance[UrgInfo.ang000Index] = (um.Distance[UrgInfo.ang000Index] < 100) ? UrgInfo.maxDist : um.Distance[UrgInfo.ang000Index];
            um.Distance[UrgInfo.ang180Index] = (um.Distance[UrgInfo.ang180Index] < 100) ? UrgInfo.maxDist : um.Distance[UrgInfo.ang180Index];
           // 添加线段
            proSeg.addPoint(
                new SimPoint() {
                    x = kp.x + Math.Cos(p.w + Math.PI) * um.Distance[UrgInfo.ang180Index] / 1000,
                    y = kp.y + Math.Sin(p.w + Math.PI) * um.Distance[UrgInfo.ang180Index] / 1000
                },
                new SimPoint() {
                    x = kp.x + Math.Cos(p.w) * um.Distance[UrgInfo.ang000Index] / 1000,
                    y = kp.y + Math.Sin(p.w) * um.Distance[UrgInfo.ang000Index] / 1000
                });
        }
        private static void showNewMap() {
            // 添加当前点以及环境信息
            MapModel m = new MapModel(DataArea.mapModel);
            m.Points.Add(PortManager.drPort.getPosition());
            if (proSeg != null) {
                m.Segments.Add(proSeg.getSegment());
            }
            // 100ms刷新一次显示
            DataArea.drawFormat.selfAdjustSize(m);
            DrawPath drawP = new DrawPath(m, DataArea.drawFormat);
            DrawPolygon drawB = new DrawPolygon(m, DataArea.drawFormat);
            if (!Form1.closing && DisplayMethod.curState == DisplayMethod.displayItem.RefPath) {
                Form1.MForms[1].Invoke(Form_Path.refreshMethod, drawP.getDrawImg(), drawB.getDrawImg());
            }
        }



    }
}
