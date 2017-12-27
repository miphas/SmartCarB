using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SmartCar {

    public class ProcessRoute {
        static GoWithSonic gowithSonic = new GoWithSonic();
        public static int sleepTime = 50;
        public static void routing() {
            // 路径文件未打开，则返回
            if (DataArea.mapModel == null) {
                MessageBox.Show("未打开路径文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 初始化串口
            PortManager.initPort();
            PortManager.openAllPort();
            PortManager.drPort.setPosition(0, 0, 0);

            // 开始巡检
            Navigation.updateLimit();
            Route(DataArea.mapModel, PortManager.conPort, PortManager.drPort, PortManager.urgPort);
        }

        /// <summary>
        /// 按照地图上点的顺序开始巡检
        /// </summary>
        /// <param name="map"></param>
        /// <param name="conPort"></param>
        /// <param name="drPort"></param>
        /// <param name="urgPort"></param>
        private static void Route(MapModel map, IConPort conPort, IDrPort drPort, IUrgPort urgPort) {
            drPort.setPosition(0, 0, 0);
            for (int i = 0; 
                ControlMethod.curState == ControlMethod.ctrlItem.GoMap && i < map.Points.Count - 1;
                ++i) {
                    if (map.Points[i + 1].type == 0) { // map.Points[i].type == 0 && 
                    KeyPoint keyDes = new KeyPoint(map.Points[i + 1]);
                    P2P(map.Points[i], keyDes, conPort, drPort);
                }
                else {
                    int keepLeft = (map.Points[i + 1].wayType == 0) ? 0 :
                                   (map.Points[i + 1].wayType == 1) ? 10 * InfoManager.wayIF.WayLeft : -10 * InfoManager.wayIF.WayLeft;
                    Forward forward = new Forward();
                    forward.Start(map.Points[i + 1], keepLeft, conPort, urgPort, drPort);
                    // 双路径时置点
                    if (map.Points[i + 1].moveBack) {
                        ++i;
                        if (map.Points.Count > i + 1) {
                            PortManager.drPort.setPosition(map.Points[i + 1]);
                        }
                    }
                    else {
                        PortManager.drPort.setPosition(map.Points[i + 1]);
                    }
                    
                }
                       
            }
        }

        /// <summary>
        /// 执行点到点编码器导航程序
        /// </summary>
        /// <param name="res"></param>
        /// <param name="des"></param>
        /// <param name="conPort"></param>
        /// <param name="drPort"></param>
        private static void P2P(KeyPoint res, KeyPoint des, IConPort conPort, IDrPort drPort)
        {
            // 设置角度、距离误差范围
            double angRound = 0.008;
            double disRound = 0.0075;
            // 开始调整的角度大小
            double angAdjust = 0.2;
            // 当前位置点信息
            KeyPoint nowPos = drPort.getPosition();

            // 按照相对坐标来走
            des.x = nowPos.x + des.x - res.x;
            des.y = nowPos.y + des.y - res.y;
            des.w = nowPos.w + des.w - res.w;

            //
            // 1.旋转AGV对准方向
            //
            int rotSpeed = 0;
            if (Math.Abs(nowPos.w - des.w) > angAdjust) {
                while (Math.Abs(nowPos.w - des.w) > angRound 
                        && ControlMethod.curState == ControlMethod.ctrlItem.GoMap) {
                    Navigation.getRotSpeed(des, nowPos, ref rotSpeed);
                    // 执行转弯
                    conPort.Control_Move_By_Speed(0, 0, rotSpeed);
                    Thread.Sleep(sleepTime);
                    nowPos = drPort.getPosition();
                }
            }

            //
            // 2.前进至目标位置
            //

            

            int goSpeed = 0;
            int shSpeed = 0;
            if (Math.Abs(res.y - des.y) < Math.Abs(res.x - des.x)) {
                while (Math.Abs(des.x - nowPos.x) > disRound && ControlMethod.curState == ControlMethod.ctrlItem.GoMap) {
                    Navigation.getDefaultSpeed(res, des, nowPos, nowPos.w + Math.PI / 2, ref goSpeed, ref shSpeed);
                    // 执行行进控制 
                    conPort.Control_Move_By_Speed(goSpeed, shSpeed, 0);
                    Thread.Sleep(sleepTime);
                    nowPos = drPort.getPosition();
                    Console.WriteLine(nowPos.x);
                }
            }
            else {
                while (Math.Abs(des.y - nowPos.y) > disRound && ControlMethod.curState == ControlMethod.ctrlItem.GoMap) {
                    Navigation.getDefaultSpeed(res, des, nowPos, nowPos.w + Math.PI / 2, ref goSpeed, ref shSpeed);
                    // 执行行进控制                    
                    conPort.Control_Move_By_Speed(goSpeed, shSpeed, 0);
                    Thread.Sleep(sleepTime);
                    nowPos = drPort.getPosition();
                }
            }
        }
        

        }
    }

