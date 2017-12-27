using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class DisplayMethod {
        // 可选当前类
        public enum displayItem {
            DoNothing, RefGuide, RefPath
        }
        // 刷新时间
        public static int refreshTime = 100;
        public static displayItem curState = displayItem.DoNothing;

        // 执行界面更新线程事项
        public static void runMethod() {
            while (!Form1.closing) {
                // 什么都不做
                if (curState == displayItem.DoNothing) {
                    // do nothing
                }
                // 更新引导界面
                else if (curState == displayItem.RefGuide) {
                    ProcessCurMap.mapping();
                }
                // 更新路径生成界面
                else if (curState == displayItem.RefPath) {
                    ProcessNewMap.mapping();
                }
                System.Threading.Thread.Sleep(refreshTime);
            }
            System.Threading.Thread.Sleep(refreshTime);
        }

    }
}
