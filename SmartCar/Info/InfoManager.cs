using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class InfoManager {
        /// <summary>
        /// 各种信息类
        /// </summary>
        public static PortInfo portIF;
        public static MoveInfo moveIF;
        public static WayInfo wayIF;
        public static CarInfo carIF;
        // 信息类
        public static Info[] Infos;

        /// <summary>
        /// 初始化各种信息类
        /// </summary>
        public static void init() {
            portIF = new PortInfo();
            moveIF = new MoveInfo();
            wayIF = new WayInfo();
            carIF = new CarInfo();
            Infos = new Info[] {
                portIF, moveIF, wayIF, carIF
            };
        }
        /// <summary>
        /// 刷新所有的信息
        /// </summary>
        public static void updateAllInfo() {
            if (portIF == null) {
                init();
            }
            foreach (var info in Infos) {
                info.updateInfo();
            }
        }

    }
}
