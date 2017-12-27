using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartCar {
    public class PortManager {
        /// <summary>
        /// 保存串口信息
        /// </summary>
        public static ConPort conPort;
        public static DrPort drPort;
        public static UrgPort urgPort;
        // 串口信息
        public static IPort[] ports;
        public static String[] portName = new String[]
        {
            "控制串口 ", "编码器串口 ", "激光雷达串口"
        };

        /// <summary>
        /// 初始化串口信息
        /// </summary>
        public static void initPort() {
            conPort = ConPort.getInstance();
            drPort = DrPort.getInstance();
            urgPort = UrgPort.getInstance();
            ports = new IPort[] {
                conPort, drPort, urgPort
            };
        }

        /// <summary>
        /// 打开所有串口
        /// </summary>
        public static void openAllPort() {
            // 无可用串口，则返回
            if (ports == null) {
                return;
            }
            // 依次打开串口
            String tip = "";
            for (int i = 0; i < ports.Length; ++i)
            {
                if (!ports[i].openPort())
                {
                    tip += portName[i];
                }
            }
            if (tip.Length != 0)
            {
                MessageBox.Show(tip + "串口打开失败，请检查串口设置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关闭所有串口
        /// </summary>
        public static void closeAllPort() {
            // 无可用串口，则返回
            if (ports == null) {
                return;
            }
            // 依次关闭可用串口
            foreach (var port in ports) {
                port.closePort();
            }
        }

    }
}
