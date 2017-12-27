using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SmartCar
{
    public class ConPort: IConPort, IPort
    {
        /////////////////////////////////////////////// public attribute ////////////////////////////////////////////

        public bool IsOpen { get { return config.port != null && config.port.IsOpen; } }
        public struct URG_POINT { public double x, y, w, d; }

        public bool IsStop { get { return config.StopSendCommand; } }

        /////////////////////////////////////////////// private attribute ////////////////////////////////////////////

        private static CONFIG config;
        private struct CONFIG
        {
            public SerialPort port;

            public bool IsReading;
            public bool IsClosing;
            public bool IsGetting;
            public bool IsSetting;

            public bool StopSendCommand;

            public byte[] Command;
            public byte[] Replay;
            public List<byte> Receive;

            public int[] UltraSonic;
        }

        /////////////////////////////////////////////// public method ////////////////////////////////////////////

        private ConPort()
        {
            config.IsReading = false;
            config.IsClosing = false;
            config.IsGetting = false;
            config.IsSetting = false;

            config.Command = new byte[11];
            config.Replay = new byte[23];
            config.Receive = new List<byte>();
            config.UltraSonic = new int[8];
        }

        /// <summary>
        /// 单例模式构造ConPort类
        /// </summary>
        private static ConPort conPort;
        public static ConPort getInstance()
        {
            if (conPort == null) {
                conPort = new ConPort();
            }
            return conPort;
        }

        public bool openPort()
        {
            // 等待串口关闭
            while (config.IsClosing) ;

            // 串口已经打开
            if (config.port != null && config.port.IsOpen) { return true; }

            // 获取串口配置
            string portName = InfoManager.portIF.ConPortName;//DataArea.InfoModel.Data[(int)FileInfo.paramE.ConPortName];
            int baudRate = InfoManager.portIF.ConPortRate;
            //try { baudRate = int.Parse(DataArea.InfoModel.Data[(int)FileInfo.paramE.ConPortRate]); }
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

        public void Control_Stop()
        {
            config.Command[7] = 0x01;
            Fill_CheckBytes(ref config.Command);
            config.StopSendCommand = true;
        }
        public void Control_Continue()
        {
            config.Command[7] = 0x00;
            Fill_CheckBytes(ref config.Command);
            config.StopSendCommand = false;
        }
        public bool Control_Move_By_Direct(int forwardSpeed, int direction, int rotateSpeed)
        {
            // 不允许发送
            if (config.StopSendCommand) { return false; }

            // 转换
            //rotateSpeed = (int)(rotateSpeed * Math.PI / 180);
            if (rotateSpeed < 0) { rotateSpeed = 128 - rotateSpeed; }

            // 限幅
            if (forwardSpeed > 800) { forwardSpeed = 800; }
            if (forwardSpeed < -800) { forwardSpeed = -800; }
            if (direction > 360) { direction = 360; }
            if (direction < 0) { direction = 0; }
            if (rotateSpeed < 0) { rotateSpeed = 0; }
            if (rotateSpeed > 255) { rotateSpeed = 255; }
            
            // 填充命令
            config.Command = new byte[11];
            config.Command[0] = 0xf1;
            config.Command[1] = 0x70;
            config.Command[2] = (byte)(forwardSpeed >> 8);
            config.Command[3] = (byte)(forwardSpeed);
            config.Command[4] = (byte)(direction >> 8);
            config.Command[5] = (byte)(direction);
            config.Command[6] = (byte)(rotateSpeed);
            config.Command[7] = 0x00;
            Fill_CheckBytes(ref config.Command);

            // 发送命令
            return sendCommand();
        }
        public bool Control_Move_By_Speed(int forwardSpeed, int leftSpeed, int rotateSpeed)
        {
            int speed = (int)Math.Sqrt(forwardSpeed * forwardSpeed + leftSpeed * leftSpeed);

            leftSpeed = -leftSpeed;

            double angle = Math.Atan((double)forwardSpeed / leftSpeed) * 180 / Math.PI;
            if (forwardSpeed > 0 && leftSpeed < 0) { angle += 180; }
            if (forwardSpeed < 0 && leftSpeed < 0) { angle += 180; }
            if (forwardSpeed < 0 && leftSpeed > 0) { angle += 360; }

            if (forwardSpeed == 0 && leftSpeed == 0) { angle = 0; }
            if (forwardSpeed == 0 && leftSpeed > 0) { angle = 0; }
            if (forwardSpeed == 0 && leftSpeed < 0) { angle = 180; }
            if (forwardSpeed > 0 && leftSpeed == 0) { angle = 90; }
            if (forwardSpeed < 0 && leftSpeed == 0) { angle = 270; }

            return Control_Move_By_Direct(speed, (int)angle, rotateSpeed);
        }

        public SonicModel Measure_Sonic()
        {
            // 等待设置完毕
            while (config.IsSetting) ;
            config.IsGetting = true;

            // 填充数据
            int[] temp = new int[8];
            for (int i = 0; i < 8; i++) { temp[i] = config.UltraSonic[i]; }
            SonicModel sonicData = new SonicModel(temp);

            // 关闭等待
            config.IsGetting = false;
            return sonicData;
        }

        /////////////////////////////////////////////// private method ////////////////////////////////////////////

        private bool sendCommand()
        {
            try
            {
                config.port.DiscardOutBuffer();
                config.port.Write(config.Command, 0, config.Command.Length);
                return true;
            }
            catch { return false; }
        }
        private void portDataReceived(object sender, EventArgs e)
        {
            // 关闭
            if (config.IsClosing || !IsOpen) { return; }

            // 正在执行读取过程
            config.IsReading = true;
            
            // 尝试读取
            try
            {
                int receLength = config.port.BytesToRead;
                byte[] TempReceive = new byte[receLength];
                config.port.Read(TempReceive, 0, receLength);

                foreach (byte ibyte in TempReceive) { config.Receive.Add(ibyte); }
            }
            catch
            {
                config.IsReading = false;
                config.Receive = new List<byte>();
                return;
            }

            // 收取阈值
            if (config.Receive.Count < config.Replay.Length) { config.IsReading = false; return; }

            // 寻找帧头
            int indexBG = -1;
            for (int i = 0; i < config.Replay.Length - 1; i++)
            {
                if (config.Receive[i] != 0xf2) { continue; }
                if (config.Receive[i + 1] != 0x70) { continue; }
                indexBG = i; break;
            }

            // 判断长度
            if (indexBG == -1) { config.Receive.Clear(); config.IsReading = false; return; }
            if (indexBG + config.Replay.Length > config.Receive.Count)
            { config.Receive.RemoveRange(0, indexBG + 1); config.IsReading = false; return; }

            // 校验帧尾
            uint sumReceived = 0;
            for (int i = 0; i < config.Replay.Length - 2; i++) { sumReceived += config.Receive[indexBG + i]; }
            sumReceived = (sumReceived >> 16) + (sumReceived & 0x0000ffff);

            byte checkH = (byte)(sumReceived >> 8);
            byte checkL = (byte)(sumReceived & 0x00ff);

            if (config.Receive[indexBG + config.Replay.Length - 2] != checkH)
            { config.Receive.RemoveRange(0, indexBG + config.Replay.Length); config.IsReading = false; return; }
            if (config.Receive[indexBG + config.Replay.Length - 1] != checkL)
            { config.Receive.RemoveRange(0, indexBG + config.Replay.Length); config.IsReading = false; return; }

            // 填充数据
            config.IsSetting = true;
            while (config.IsGetting) ;

            int BitBG = indexBG + 5;

            config.UltraSonic[0] = (config.Receive[BitBG + 00]) << 8 | config.Receive[BitBG + 01];
            config.UltraSonic[1] = (config.Receive[BitBG + 02]) << 8 | config.Receive[BitBG + 03];
            config.UltraSonic[2] = (config.Receive[BitBG + 04]) << 8 | config.Receive[BitBG + 05];
            config.UltraSonic[3] = (config.Receive[BitBG + 06]) << 8 | config.Receive[BitBG + 07];
            config.UltraSonic[4] = (config.Receive[BitBG + 08]) << 8 | config.Receive[BitBG + 09];
            config.UltraSonic[5] = (config.Receive[BitBG + 10]) << 8 | config.Receive[BitBG + 11];
            config.UltraSonic[6] = (config.Receive[BitBG + 12]) << 8 | config.Receive[BitBG + 13];
            config.UltraSonic[7] = (config.Receive[BitBG + 14]) << 8 | config.Receive[BitBG + 15];

            config.IsSetting = false;
            config.Receive.RemoveRange(0, indexBG + config.Replay.Length);
            config.IsReading = false;
        }
        private static void Fill_CheckBytes(ref byte[] command)
        {
            uint sumCommand = 0;
            for (int i = 0; i < command.Length - 2; i++) { sumCommand += command[i]; }

            sumCommand = (sumCommand >> 16) + (sumCommand & 0x0000ffff);

            command[command.Length - 2] = (byte)(sumCommand >> 8);
            command[command.Length - 1] = (byte)(sumCommand & 0x000000ff);
        }
    }
}
