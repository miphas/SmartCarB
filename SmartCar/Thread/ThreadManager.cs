using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SmartCar {
    public class ThreadManager {
        // 控制命令线程
        public static Thread conThread = new Thread(new ThreadStart(ControlMethod.runMethod));
        // 刷新界面线程
        public static Thread disThread = new Thread(new ThreadStart(DisplayMethod.runMethod));

        public static void start() {
            conThread.Start();
            disThread.Start();
        }

        public static void end() {
            conThread.Abort();
            disThread.Abort();
        }

    }
}
