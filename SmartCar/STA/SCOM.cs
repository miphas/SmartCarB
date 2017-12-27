using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class SCOM {
        /// <summary>
        /// 串口编号选择
        /// </summary>
        public static String[] comName = new String[] {
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10",
            "COM11", "COM12", "COM13", "COM14", "COM15", "COM16", "COM17", "COM18", "COM19", "COM20"
        };
        public enum comE {
            COM1, COM2, COM3, COM4, COM5, COM6, COM7, COM8, COM9, COM10,
            COM11, COM12, COM13, COM14, COM15, COM16, COM17, COM18, COM19, COM20
        };

        /// <summary>
        /// 串口波特率
        /// </summary>
        public static String[] ratName = new String[] {
            "9600", "115200"
        };
        public enum ratE {
            R9600, R115200
        }

    }
}
