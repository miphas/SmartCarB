using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using SCIP_library;

namespace SmartCar
{
    public class UrgPort : IUrgPort, IPort
    {
        /////////////////////////////////////////////// public attribute ////////////////////////////////////////////

        /// <summary>
        /// 串口状态
        /// </summary>
        public bool IsOpen { get { return config.port != null && config.port.IsOpen; } }
        /// <summary>
        /// 线程状态
        /// </summary>
        public bool TH_running { get { return config.TH_urg != null && config.TH_urg.ThreadState == System.Threading.ThreadState.Running; } }
        /// <summary>
        /// 起始角度
        /// </summary>
        public double AngleStart { get { return config.AngleStart; } }
        /// <summary>
        /// 角度跨度
        /// </summary>
        public double AnglePace { get { return config.AnglePace; } }

        /////////////////////////////////////////////// private attribute ////////////////////////////////////////////

        private static CONFIG config;

        private struct CONFIG
        {
            public SerialPort port;
            public bool IsReading;
            public bool IsClosing;
            public bool IsSetting;
            public bool IsGetting;

            public System.Threading.Thread TH_urg;
            public bool TH_cmd_abort;

            public int ReceiveBG;
            public int ReceiveED;
            public int CutBG;
            public int CutED;
            public double AngleStart;
            public double AnglePace;

            public List<long> Receive;
            public long TimeStamp;

            public long[] Distance;
        }

        /////////////////////////////////////////////// public method ////////////////////////////////////////////

        private UrgPort()
        {
            config.IsReading = false;
            config.IsClosing = false;
            config.IsSetting = false;
            config.IsGetting = false;

            config.TH_cmd_abort = false;

            config.ReceiveBG = 0;
            config.ReceiveED = 760;
            config.CutBG = 44;
            config.CutED = 726;
            config.AngleStart = -30.0;
            config.AnglePace = 360.0 / 1024.0;

            config.Receive = new List<long>();
            config.Distance = new long[0];
        }
        /// <summary>
        /// 激光雷达串口
        /// </summary>
        private static UrgPort urgPort;
        public static UrgPort getInstance()
        {
            if (urgPort == null) {
                urgPort = new UrgPort();
            }
            return urgPort;
        }

        public bool openPort()
        {
            // 等待串口关闭
            while (config.IsClosing) ;

            // 串口已经打开，重新开启线程
            if (config.port != null && config.port.IsOpen) { return true; }

            // 获取串口配置
            string portName = InfoManager.portIF.UrgPortName;//DataArea.InfoModel.Data[(int)FileInfo.paramE.UrgPortName];
            int baudRate = InfoManager.portIF.UrgPortRate; ;
            //try { baudRate = int.Parse(DataArea.InfoModel.Data[(int)FileInfo.paramE.UrgPortRate]); }
            //catch { return false; }
            config.port = new SerialPort(portName, baudRate);

            // 尝试打开串口
            try
            {
                config.port.NewLine = "\n\n";
                config.port.Open();
                config.port.Write(SCIP_Writer.SCIP2());
                config.port.ReadLine();
                config.port.Write(SCIP_Writer.MD(config.ReceiveBG, config.ReceiveED));
                config.port.ReadLine();
            }
            catch { return false; }

            // 重新开启线程
            return Start_TH_urg();
        }
        public bool closePort()
        {
            if (config.port == null || !config.port.IsOpen) { return true; }

            config.IsClosing = true;
            while (config.IsReading) ;

            try
            {
                config.port.Write(SCIP_Writer.QT());
                config.port.ReadLine();
                config.port.Close();
                config.IsClosing = false;
                return true;
            }
            catch
            {
                config.IsClosing = false;
                return false;
            }
        }

        public UrgModel getUrgData()
        {
            while (config.IsSetting) ;
            config.IsGetting = true;

            List<long> Distance = new List<long>();
            foreach (long idis in config.Distance) { Distance.Add(idis); }

            config.IsGetting = false;

            UrgModel urgModel = new UrgModel();
            urgModel.Distance = Distance;
            return urgModel;
        }

        /////////////////////////////////////////////// private method ////////////////////////////////////////////

        private void RefreshUrgData()
        {
            Util.ExpData.createFile();
            while (true)
            {
                // 如果串口关闭，则线程结束
                if (config.port == null || !config.port.IsOpen || config.IsClosing)
                { config.TH_urg.Abort(); config.TH_cmd_abort = false; return; }

                // 外部要求关闭线程
                if (config.TH_cmd_abort) { config.TH_urg.Abort(); config.TH_cmd_abort = false; return; }

                // 延时 （无需延时，否则周期接近200ms）
                // System.Threading.Thread.Sleep(100);
                
                // 收取数据
                if (!portDataReceived()) { config.IsSetting = false; continue; }

                // 滤波
                // experiment恢复原始数据
                // UrgFilter();
                // 
                // Console.WriteLine(Environment.TickCount);
                Util.ExpData.writeData(config.Receive, 1, 2, 3);

                // 设置完毕
                config.IsSetting = true;
                while (config.IsGetting) ;

                config.Distance = config.Receive.ToArray();

                config.IsSetting = false;
            }
        }

        private bool Start_TH_urg()
        {
            try
            {
                config.TH_cmd_abort = true;
                while (config.TH_urg != null && config.TH_urg.ThreadState == System.Threading.ThreadState.Running) ;
                config.TH_cmd_abort = false;

                config.TH_urg = new System.Threading.Thread(RefreshUrgData);
                config.TH_urg.Start();
            }
            catch { return false; }

            return true;
        }
        private bool portDataReceived()
        {
            if (config.IsClosing || !IsOpen) { return false; }
            config.IsReading = true;
















             













            config.port.DiscardInBuffer();
            string receiveData = config.port.ReadLine();
            config.Receive = new List<long>();
            
            if (!SCIP_Reader.MD(receiveData, ref config.TimeStamp, ref config.Receive))
            {
                Console.WriteLine(receiveData);
                return false;
            }
            if (config.Receive.Count == 0)
            {
                Console.WriteLine(receiveData);
                return false;
            }

            config.Receive.RemoveRange(config.CutED, config.Receive.Count - config.CutED);
            config.Receive.RemoveRange(0, config.CutBG);

            config.IsReading = false;
            return true;
        }
        private void UrgFilter()
        {
            for (int i = 0; i < config.Receive.Count; i++)
            { if (config.Receive[i] < 100) { config.Receive[i] = 0; } }
            
            int N_nege = 5, N = config.Receive.Count;
            double floatError = 100;

            List<long> diff = new List<long>();
            for (int i = 1; i < N; i++) { diff.Add(Math.Abs(config.Receive[i] - config.Receive[i - 1])); }

            List<int> P = new List<int>();
            for (int i = 0; i < N - 1; i++) { if (diff[i] > floatError) { P.Add(i); } }

            for (int i = 0; i < P.Count - 1; i++)
            {
                if (P[i + 1] - P[i] > N_nege) { continue; }
                for (int j = P[i] + 1; j <= P[i + 1]; j++) { config.Receive[j] = 0; }
            }
        }

    }
}
