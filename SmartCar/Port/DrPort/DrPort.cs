using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SmartCar
{
    public class DrPort:IDrPort,IPort
    {
        /////////////////////////////////////////////// public attribute ////////////////////////////////////////////

        public bool IsOpen { get { return config.port != null && config.port.IsOpen; } }

        /////////////////////////////////////////////// private attribute ////////////////////////////////////////////

        private static CONFIG config;

        private struct CONFIG
        {
            public SerialPort port;

            public bool IsReading;
            public bool IsClosing;
            public bool IsGetting;
            public bool IsSetting;

            public SPEED CurrSpeed;
            public SPEED PrevSpeed;

            public List<byte> Receive;
            public byte[] Frame;

            public double X, Y, W;

            public struct SPEED { public int HeadL, HeadR, TailL, TailR; }
        }

        /////////////////////////////////////////////// public method ////////////////////////////////////////////

        private DrPort()
        {
            config.IsReading = false;
            config.IsClosing = false;
            config.IsGetting = false;
            config.IsSetting = false;

            config.Receive = new List<byte>();
            config.Frame = new byte[14];
        }
        /// <summary>
        /// 单例模式构造类
        /// </summary>
        private static DrPort drPort;
        public static DrPort getInstance() {
            if (drPort == null) {
                drPort = new DrPort();
            }
            return drPort;
        }

        public bool openPort()
        {
            // 等待串口关闭
            while (config.IsClosing) ;

            // 串口已经打开
            if (config.port != null && config.port.IsOpen) { return true; }

            // 获取串口配置
            string portName = InfoManager.portIF.DrPortName;//DataArea.infoModel.Data[(int)FileInfo.paramE.DrPortName];
            int baudRate = InfoManager.portIF.DrPortRate;
            //try { baudRate = int.Parse(DataArea.infoModel.Data[(int)FileInfo.paramE.DrPortRate]); }
            //catch { return false; }

            // 尝试打开串口
            try
            {
                config.port = new SerialPort(portName, baudRate);
                config.port.DataReceived -= portDataReceived;
                config.port.DataReceived += portDataReceived;
                config.port.Open();
            }
            catch { return false; }

            // 返回。
            return true;
        }
        public bool closePort()
        {
            // 串口已经释放
            if (config.port == null || !config.port.IsOpen) { return true; }

            // 等待读取数据完毕
            config.IsClosing = true;
            while (config.IsReading) ;

            // 尝试关闭串口
            bool closed = true;
            try { config.port.Close(); } catch { closed = false; }

            // 返回
            config.IsClosing = false;
            return closed;
        }

        public KeyPoint getPosition()
        {
            while (config.IsSetting) ;
            config.IsGetting = true;

            KeyPoint keyPoint = new KeyPoint();
            keyPoint.x = config.X;
            keyPoint.y = config.Y;
            keyPoint.w = config.W;

            config.IsGetting = false;

            return keyPoint;
        }
        public void setPosition(KeyPoint keyPoint)
        {
            setPosition(keyPoint.x, keyPoint.y, keyPoint.w);
        }
        public void setPosition(double x, double y, double w)
        {
            while (config.IsSetting) ;
            while (config.IsGetting) ;
            config.IsSetting = true;
            config.IsGetting = true;

            config.X = x;
            config.Y = y;
            config.W = w;

            config.IsSetting = false;
            config.IsGetting = false;
        }

        /////////////////////////////////////////////// private method ////////////////////////////////////////////

        private void portDataReceived(object sender, EventArgs e)
        {
            // 正在关闭
            if (config.IsClosing || !IsOpen) { return; }

            // 正在读取
            config.IsReading = true;

            // 尝试接收数据
            try
            {
                int receLength = config.port.BytesToRead;
                byte[] Temp = new byte[receLength];

                config.port.Read(Temp, 0, receLength);
                foreach (byte ibyte in Temp) { config.Receive.Add(ibyte); }
            }
            catch { config.IsReading = false; config.Receive.Clear(); return; }

            // 寻找完整的帧，并对应做出处理
            int indexBG = -1, indexED = config.Receive.Count;
            for (int i = 0; i < config.Receive.Count; i++)
            {
                // 找到完整的一帧
                if (config.Receive[i] != 0xAA) { continue; }
                indexBG = i;
                if (i + config.Frame.Length > config.Receive.Count) { break; }
                if (config.Receive[indexBG + config.Frame.Length - 1] != 0xBB) { continue; }
                for (int j = 0; j < config.Frame.Length; j++) { config.Frame[j] = config.Receive[indexBG + j]; }

                // 帧尾
                indexED = indexBG + config.Frame.Length;

                // 对这一帧进行处理
                getCurrentSpeed();
                getCurrentPosition();
            }

            // 丢弃已经处理的数据
            config.Receive.RemoveRange(0, indexED);
            config.IsReading = false;
        }

        private static void getCurrentSpeed()
        {
            int H, L;

            H = config.Frame[2];
            L = config.Frame[3];
            config.CurrSpeed.HeadL = (H << 8) | L;

            H = config.Frame[5];
            L = config.Frame[6];
            config.CurrSpeed.TailL = (H << 8) | L;

            H = config.Frame[8];
            L = config.Frame[9];
            config.CurrSpeed.TailR = (H << 8) | L;

            H = config.Frame[11];
            L = config.Frame[12];
            config.CurrSpeed.HeadR = (H << 8) | L;

            if (config.Frame[1] == 0) { config.CurrSpeed.HeadL = -config.CurrSpeed.HeadL; }
            if (config.Frame[4] == 0) { config.CurrSpeed.TailL = -config.CurrSpeed.TailL; }
            if (config.Frame[7] == 0) { config.CurrSpeed.TailR = -config.CurrSpeed.TailR; }
            if (config.Frame[10] == 0) { config.CurrSpeed.HeadR = -config.CurrSpeed.HeadR; }

            int StopAmount = 0;
            if (config.CurrSpeed.HeadL == 0) { StopAmount++; }
            if (config.CurrSpeed.HeadR == 0) { StopAmount++; }
            if (config.CurrSpeed.TailL == 0) { StopAmount++; }
            if (config.CurrSpeed.TailR == 0) { StopAmount++; }
            if (StopAmount < 2) { return; }

            config.CurrSpeed.HeadL = 0;
            config.CurrSpeed.HeadR = 0;
            config.CurrSpeed.TailL = 0;
            config.CurrSpeed.TailR = 0;
        }
        private static void getCurrentPosition()
        {
            int currSpeed_HL = -config.CurrSpeed.HeadL;
            int currSpeed_HR = config.CurrSpeed.HeadR;
            int currSpeed_TL = -config.CurrSpeed.TailL;
            int currSpeed_TR = config.CurrSpeed.TailR;

            const double R = 0.1015, PI = 3.1416, Lx = 0.1825, Ly = 0.2950;
            //编码器修正 ，HR数据出错
            if (currSpeed_HL * currSpeed_HR * currSpeed_TL * currSpeed_TR < 0)
            {
                currSpeed_HR = currSpeed_HL;
                currSpeed_TL = currSpeed_HL;
                currSpeed_TR = currSpeed_HL;
            }
            if ((Math.Abs(currSpeed_HL - currSpeed_TL) < 20 && Math.Abs(currSpeed_HL - currSpeed_TR) < 20 && Math.Abs(currSpeed_TL - currSpeed_TR) < 20) || (Math.Abs(currSpeed_HL - currSpeed_TR) < 20 && Math.Abs(currSpeed_HL - currSpeed_TL) < 20 && Math.Abs(currSpeed_TL - currSpeed_HL) < 20))
            {
                //前进倒退
                if ((currSpeed_HL > 0 && currSpeed_HR > 0 && currSpeed_TL > 0 && currSpeed_TR > 0) || (currSpeed_HL < 0 && currSpeed_HR < 0 && currSpeed_TL < 0 && currSpeed_TR < 0))
                {
                    currSpeed_HR = currSpeed_HL;
                    currSpeed_TL = currSpeed_HL;
                    currSpeed_TR = currSpeed_HL;
                }
                //左右平移
                else if ((currSpeed_HL > 0 && currSpeed_TL < 0 && currSpeed_TR > 0 && currSpeed_HR < 0) || (currSpeed_HL < 0 && currSpeed_HR > 0 && currSpeed_TL > 0 && currSpeed_TR < 0))
                {
                    currSpeed_HR = -currSpeed_HL;
                    currSpeed_TL = -currSpeed_HL;
                    currSpeed_TR = currSpeed_HL;
                }
                else if ((currSpeed_HL < 0 && currSpeed_TL < 0 && currSpeed_TR > 0 && currSpeed_HR > 0) || (currSpeed_HL > 0 && currSpeed_TL > 0 && currSpeed_HR < 0 && currSpeed_TR < 0))
                {
                    currSpeed_HR = -currSpeed_HL;
                    currSpeed_TL = currSpeed_HL;
                    currSpeed_TR = -currSpeed_HL;
                }

            }



            double X = (double)(currSpeed_TL + currSpeed_HR - currSpeed_HL - currSpeed_TR) * 2 * PI * R / 1000 / 4;
            double Y = (double)(currSpeed_TL + currSpeed_TR + currSpeed_HL + currSpeed_HR) * 2 * PI * R / 1000 / 4;
            double W = (double)(currSpeed_TR + currSpeed_HR - currSpeed_TL - currSpeed_HL) * 2 * PI * R / 1000 / (4 * (Lx + Ly));
             
            X = X / 4.077;
            Y = Y / 4.077;
            W = W / 4.077;

            config.IsSetting = true;
            while (config.IsGetting) ;

            config.X += X * Math.Cos(config.W) - Y * Math.Sin(config.W);
            config.Y += X * Math.Sin(config.W) + Y * Math.Cos(config.W);
            config.W += W;

            config.IsSetting = false;
        }
    }
}
