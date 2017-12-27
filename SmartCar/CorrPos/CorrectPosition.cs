using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    class CorrectPosition:ICorrectPosition
    {
        /////////////////////////////////////////////// public attribute ////////////////////////////////////////////

        /////////////////////////////////////////////// private attribute ////////////////////////////////////////////

        private static CONFIG config;
        private struct CONFIG
        {
            public List<URG_POINT> pointsH;
            public List<List<URG_POINT>> urgGroups;

            public double xError;
            public double aError;
            public int TimeForControl;
        }
        private struct URG_POINT { public double x, y, a, d; }

        /////////////////////////////////////////////// public method ////////////////////////////////////////////

        public KeyPoint getCurrentKB(UrgPort urgPort)
        {
            KeyPoint keyPoint = new KeyPoint();
            keyPoint.Can_Adj = 0;
            if (!urgPort.IsOpen) { return keyPoint; }

            // 取得数据
            getUrgPoint(urgPort);

            // 分割成组
            config.urgGroups = new List<List<URG_POINT>>();
            CutGroup_UrgPoint(config.pointsH);

            // 挑选出正前方数据，进行拟合
            List<URG_POINT> linePoints = GetHeadGroup_UrgPoint();
            if (linePoints.Count <= 3) { return keyPoint; }
            double[] KB = Fit_UrgPoint(linePoints);
            keyPoint.UrgK = KB[0];
            keyPoint.UrgB = KB[1];

            if (keyPoint.UrgB < 1200) { keyPoint.Can_Adj = 1; }
            return keyPoint;
        }

        public void Start(ConPort conPort, UrgPort urgPort, KeyPoint keyPoint)
        {
            while (getCurrentKB(urgPort).UrgB == 0) ;

            config.TimeForControl = 100;

            config.xError = 20;
            config.aError = 2;
            Adjust_A(conPort, urgPort, keyPoint);
            Adjust_X(conPort, urgPort, keyPoint);

            config.xError = 5;
            config.aError = 0.5;
            Adjust_A(conPort, urgPort, keyPoint);
            Adjust_X(conPort, urgPort, keyPoint);
        }

        /////////////////////////////////////////////// private attribute //////////////////////////////////////////// 
        
        private bool getUrgPoint(UrgPort urgPort)
        {
            config.pointsH = new List<URG_POINT>();
            if (!urgPort.IsOpen) { return false; }
            
            UrgModel urgModel = urgPort.getUrgData();
            if (urgModel.Distance == null) { return false; }
            int BG = (int)((75 - urgPort.AngleStart) / urgPort.AnglePace);
            int ED = (int)((105 - urgPort.AngleStart) / urgPort.AnglePace);
            if (urgModel.Distance.Count < ED) { return false; }

            config.pointsH = new List<URG_POINT>();

            for (int i = BG; i < ED; i++)
            {
                if (urgModel.Distance[i] == 0) { continue; }

                URG_POINT point = new URG_POINT();
                point.d = urgModel.Distance[i];

                double angle = urgPort.AngleStart + i * urgPort.AnglePace;

                point.a = angle;
                point.x = point.d * Math.Cos(angle * Math.PI / 180);
                point.y = point.d * Math.Sin(angle * Math.PI / 180);

                config.pointsH.Add(point);
            }

            return true;
        }
        private void CutGroup_UrgPoint(List<URG_POINT> points)
        {
            // 点的数量不够
            if (points.Count == 0) { return; }
            if (points.Count == 1 || points.Count == 2) { config.urgGroups.Add(points); return; }

            // 基本参数
            double MaxDis = 0.0;
            int indexofmax = 0;

            // 直线参数
            double x1 = points[0].x, y1 = points[0].y;
            double x2 = points[points.Count - 1].x, y2 = points[points.Count - 1].y;

            double A = y2 - y1, B = -(x2 - x1), C = (x2 - x1) * y1 - (y2 - y1) * x1;

            // 寻找最大距离
            for (int i = 0; i < points.Count; i++)
            {
                double iDis = (A * points[i].x + B * points[i].y + C) / Math.Sqrt(A * A + B * B);
                if (MaxDis > iDis) { continue; }

                indexofmax = i; MaxDis = iDis;
            }

            // 分割直线
            if (MaxDis <= 30) { config.urgGroups.Add(points); return; }

            List<URG_POINT> newLine = new List<URG_POINT>();
            for (int i = 0; i <= indexofmax; i++) { newLine.Add(points[i]); }
            CutGroup_UrgPoint(newLine);

            newLine = new List<URG_POINT>();
            for (int i = indexofmax; i < points.Count; i++) { newLine.Add(points[i]); }
            CutGroup_UrgPoint(newLine);
        }
        private List<URG_POINT> GetHeadGroup_UrgPoint()
        {
            if (config.urgGroups.Count == 0) { return new List<URG_POINT>(); }

            // 挑选直线
            for (int i = 0; i < config.urgGroups.Count; i++)
            {
                double bgX = config.urgGroups[i][0].x;
                double edX = config.urgGroups[i][config.urgGroups[i].Count - 1].x;

                if (bgX > 0 && edX < 0) { return config.urgGroups[i]; }
                if (bgX < 0 && edX > 0) { return config.urgGroups[i]; }

                if (bgX == 0)
                {
                    if (i == 0) { return config.urgGroups[i]; }
                    if (config.urgGroups[i].Count >= config.urgGroups[i - 1].Count) { return config.urgGroups[i]; }
                    return config.urgGroups[i - 1];
                }
                if (edX == 0)
                {
                    if (i == config.urgGroups.Count) { return config.urgGroups[i]; }
                    if (config.urgGroups[i].Count >= config.urgGroups[i + 1].Count) { return config.urgGroups[i]; }
                    return config.urgGroups[i + 1];
                }
            }
            return config.urgGroups[0];
        }
        private double[] Fit_UrgPoint(List<URG_POINT> points)
        {
            if (points.Count <= 3) { return new double[3] { 0, 0, 0 }; }

            double sumX = 0, sumY = 0, sumXX = 0, sumYY = 0, sumXY = 0;
            int N = points.Count;

            for (int i = 0; i < N; i++)
            {
                sumX += points[i].x;
                sumY += points[i].y;

                sumXX += points[i].x * points[i].x;
                sumXY += points[i].x * points[i].y;
                sumYY += points[i].y * points[i].y;
            }

            double denominator = N * sumXX - sumX * sumX;
            if (denominator == 0) { denominator = 0.0000001; }

            double UrgK = (N * sumXY - sumX * sumY) / denominator;
            double UrgB = (sumXX * sumY - sumX * sumXY) / denominator;

            UrgK = Math.Atan(UrgK) * 180 / Math.PI;
            return new double[2] { UrgK, UrgB };
        }

        private void Adjust_A(ConPort conPort, UrgPort urgPort, KeyPoint targetPoint)
        {
            KeyPoint currentPoint = getCurrentKB(urgPort);

            while (true)
            {
                // 比较精度
                double current = currentPoint.UrgK;
                double target = targetPoint.UrgK;
                if (Math.Abs(current - target) <= config.aError) { return; }

                // 获取控制速度
                double adjustA = (current - target) * 150;
                int aSpeed = (int)(adjustA /config.TimeForControl);
                if (aSpeed == 0) { return; }

                // 限幅
                if (aSpeed > 25) { aSpeed = 25; }
                if (aSpeed < -25) { aSpeed = -25; }

                // 控制
                conPort.Control_Move_By_Speed(0, 0, aSpeed);

                // 更新数据
                System.Threading.Thread.Sleep(config.TimeForControl);
                currentPoint = getCurrentKB(urgPort);
            }
        }
        private void Adjust_X(ConPort conPort, UrgPort urgPort, KeyPoint targetPoint)
        {
            KeyPoint currentPoint = getCurrentKB(urgPort);

            while (true)
            {
                // 比较精度
                double current = currentPoint.UrgB;
                double target = targetPoint.UrgB;
                if (Math.Abs(current - target) < config.xError) { return; }

                // 获得输出
                double adjustX = (current - target)*40;
                int xSpeed = (int)(adjustX / config.TimeForControl);
                if (xSpeed == 0) { return; }

                // 限幅
                if (xSpeed > 300) { xSpeed = 300; }
                if (xSpeed < -300) { xSpeed = -300; }

                // 控制
                conPort.Control_Move_By_Speed(xSpeed, 0, 0);

                // 更新数据
                System.Threading.Thread.Sleep(config.TimeForControl);
                currentPoint = getCurrentKB(urgPort);
            }
        }
    }
}
