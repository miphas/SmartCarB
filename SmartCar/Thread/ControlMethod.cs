using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class ControlMethod {

        // 可选当前类
        public enum ctrlItem {
            DoNothing, GoMap, ExpMap
        }
        // 刷新时间
        public static int refreshTime = 100;
        public static ctrlItem curState = ctrlItem.DoNothing;
        
        // 执行控制线程事项
        public static void runMethod() {
            
            while (!Form1.closing) {
                // 什么都不做
                if (curState == ctrlItem.DoNothing) {
                    // do nothing
                }
                // 沿地图行进
                else if (curState == ctrlItem.GoMap) {
                    ProcessRoute.routing();
                    curState = ctrlItem.DoNothing;
                }
                // 沿通道创建地图 
                
                else if (curState == ctrlItem.ExpMap) {
                    // 沿通道行进倒退
                    new Forward().Start(new KeyPoint(), 0, PortManager.conPort, PortManager.urgPort, PortManager.drPort);
                    // 重设关键点(仅双路径时重设)
                    if (Form_Path.wayBack) {
                        PortManager.drPort.setPosition(DataArea.mapModel.Points[DataArea.mapModel.Points.Count - 2]);
                        ProcessNewMap.markKeyPoint(0);
                    }
                    

                    curState = ctrlItem.DoNothing;
                }
            }
            System.Threading.Thread.Sleep(refreshTime);
        }

    }
}
