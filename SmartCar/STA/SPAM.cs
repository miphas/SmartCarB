using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class SPAM {
        /// <summary>
        /// 文件节点信息
        /// </summary>
        public static String[] paramName = new String[] {
            "ConPortName", "ConPortRate", "DrPortName", "DrPortRate", "UrgPortName", "UrgPortRate", "MaxFront", "MaxSide", "MaxRotate", "WayWidth", "WayLeft", "WayRight", "CarW", "CarH", "RadTop", "RadLeft"
        };
        public static String[] paramDef = new String[] {
              "COM3",        "115200",       "COM4",       "115200",      "COM5",       "115200",      "60",      "80",      "20",          "80",       "20",      "20",    "44",   "140",   "3",      "22"
        };
        public enum paramE {
             ConPortName,  ConPortRate,    DrPortName,   DrPortRate,   UrgPortName,   UrgPortRate,   MaxFront,   MaxSide,   MaxRotate,    WayWidth,  WayLeft,   WayRight,    CarW,  CarH,   RadTop,   RadLeft
        };

    }
}
