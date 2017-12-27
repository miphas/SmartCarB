using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    class Forward
    {
        public static int num;
        private static CONFIG config;
        private struct CONFIG
        {
            public bool AchieveTarget;

            public int MaxForwardSpeed;
            public int MaxTranslateSpeed;
            public int MaxRotateSpeed;
            
            public PD_PARAMETER PD_F, PD_T, PD_R;

            public KeyPoint PreviousPos;
            
            public struct PD_PARAMETER { public double Kp, Kd; public double Error2, Error1, Error0; }
            public struct URG_POINT { public double x, y, a, d; }
        }

        public Forward()
        {
            // 100 50 10 1 0.2 0.3

            config.AchieveTarget = false;
            config.MaxForwardSpeed = InfoManager.moveIF.MaxFront;//int.Parse(DataArea.infoModel.Data[(int)FileInfo.paramE.MaxFront]);
            config.MaxTranslateSpeed = 30;
            config.MaxRotateSpeed = 5;

            config.PD_F.Kp = 1;
            config.PD_F.Kd = 0;
            config.PD_F.Error0 = 0;
            config.PD_F.Error1 = 0;
            config.PD_F.Error2 = 0;

            config.PD_T.Kp = 0.3;
            config.PD_T.Kd = 0;
            config.PD_T.Error0 = 0;
            config.PD_T.Error1 = 0;
            config.PD_T.Error2 = 0;

            config.PD_R.Kp =  0.3;
            config.PD_R.Kd = 0;
            config.PD_R.Error0 = 0;
            config.PD_R.Error1 = 0;
            config.PD_R.Error2 = 0;

            config.AchieveTarget = false;
        }

        public void Start(KeyPoint targetPoint, double keepLeft, IConPort conPort, IUrgPort urgPort, IDrPort drPort)
        {
            if (ControlMethod.curState == ControlMethod.ctrlItem.ExpMap) {
                int setLeft = InfoManager.wayIF.WayLeft * 10;//10 * int.Parse(DataArea.infoModel.Data[(int)FileInfo.paramE.WayLeft]);
                int setRigh = InfoManager.wayIF.WayRight * 10;//10 * int.Parse(DataArea.infoModel.Data[(int)FileInfo.paramE.WayRight]);
                keepLeft = (Form_Path.wayType == 0) ? 0 :
                           (Form_Path.wayType == 1) ? setLeft : -setRigh;
            }

            // 记录距离，自己决定开启
            AlignAisle align = new AlignAisle();
            double record = align.recordDistance();
            align.Start();

            Backward backward = new Backward();
            backward.clear();

            config.PreviousPos = drPort.getPosition();

            #region 找到通道入口

            while (true)
            {
                // 获取速度
                int ySpeed = getForwardSpeed(config.MaxForwardSpeed, targetPoint, urgPort, drPort);
                int xSpeed = 0;
                int wSpeed = 0;

                // 退出条件
                List<CONFIG.URG_POINT> pointsL = getUrgPoint(160, 180, urgPort);
                List<CONFIG.URG_POINT> pointsR = getUrgPoint(0, 20, urgPort);

                double minL = double.MaxValue, minR = double.MaxValue;
                for (int i = 0; i < pointsL.Count; i++)
                {
                    double x = Math.Abs(pointsL[i].x);
                    if (x < minL) { minL = x; }
                }
                for (int i = 0; i < pointsR.Count; i++)
                {
                    double x = Math.Abs(pointsR[i].x);
                    if (x < minR) { minR = x; }
                }

                if (minL < 1000 || minR < 1000) { break; }

                // 控制
                conPort.Control_Move_By_Speed(ySpeed, xSpeed, wSpeed);

                // 比较之前与现在的位置
                KeyPoint currentPos = drPort.getPosition();

                bool recored = currentPos.x != config.PreviousPos.x ||
                    currentPos.y != config.PreviousPos.y ||
                    currentPos.w != config.PreviousPos.w;

                config.PreviousPos = currentPos;

                if (!PortManager.conPort.IsStop && recored) {
                    Backward.COMMAND command = new Backward.COMMAND();
                    command.ForwardSpeed = ySpeed;
                    command.LeftSpeed = xSpeed;
                    command.RotateSpeed = wSpeed;
                    backward.set(command);
                }

                

                System.Threading.Thread.Sleep(100);
            }

            #endregion

            //backward.startpoint = drPort.getPosition();

            #region 通道内行走

            if (keepLeft < 0) { keepLeft -= 225; }
            if (keepLeft > 0) { keepLeft += 225; }

            while (!config.AchieveTarget)
            {
                int ForwardSpeed = getForwardSpeed(config.MaxForwardSpeed, targetPoint, urgPort, drPort);
                int TranslateSpeed = -getTranslateSpeed(keepLeft,conPort,urgPort, drPort);
                int RotateSpeed = getRotateSpeed(conPort, urgPort, drPort);

                // 距离限速
                List<CONFIG.URG_POINT> pointsH = getUrgPoint(85, 95, urgPort);
                double minH = double.MaxValue;
                for (int i = 0; i < pointsH.Count; i++)
                { if (minH > pointsH[i].y) { minH = pointsH[i].y; } }
                if (minH < 1200)
                {
                    TranslateSpeed = 0;
                    //RotateSpeed = 0;
                }


                double current = drPort.getPosition().y;
                while (Math.Abs(current - targetPoint.y) < 0.02) { break; }
                
                conPort.Control_Move_By_Speed(ForwardSpeed, TranslateSpeed, RotateSpeed);

                // 比较之前与现在的位置
                KeyPoint currentPos = drPort.getPosition();

                bool recored = currentPos.x != config.PreviousPos.x ||
                    currentPos.y != config.PreviousPos.y ||
                    currentPos.w != config.PreviousPos.w;

                config.PreviousPos = currentPos;

                if (!PortManager.conPort.IsStop && recored)
                {
                    Backward.COMMAND command = new Backward.COMMAND();
                    command.ForwardSpeed = ForwardSpeed;
                    command.LeftSpeed = TranslateSpeed;
                    command.RotateSpeed = RotateSpeed;
                    backward.set(command);
                }



                System.Threading.Thread.Sleep(100);
            }

            #endregion

            if (ControlMethod.curState == ControlMethod.ctrlItem.ExpMap) {
                ProcessNewMap.markKeyPoint(1, record);
            }
            

            // 校准方式1
            //CorrPos(targetPoint, conPort, drPort);

            // 校准方式2
            //CorrectPosition corrp = new CorrectPosition();
            //corrp.Start(PortManager.conPort, PortManager.urgPort, targetPoint);
            //PortManager.drPort.setPosition(targetPoint);

            // 单一路径返回
            if (ControlMethod.curState == ControlMethod.ctrlItem.ExpMap && !Form_Path.wayBack) {
                return;
            }
            if (ControlMethod.curState == ControlMethod.ctrlItem.GoMap && !targetPoint.moveBack) {
                return;
            }

            // 后退
            conPort.Control_Move_By_Speed(0, 0, 0);
            System.Threading.Thread.Sleep(1000);
            backward.Start();

            // 调整距离，自己决定开启
            if (ControlMethod.curState == ControlMethod.ctrlItem.GoMap)
            {
                // 大于10mm开启调整
                if (targetPoint.disWay > 10)
                {
                    align.adjustDistance(targetPoint.disWay);
                }
            }
            else if (ControlMethod.curState == ControlMethod.ctrlItem.ExpMap)
            {
                align.adjustDistance(record);
            }

        }

        public void CorrPos(KeyPoint targetPoint, IConPort conPort, IDrPort drPort)
        {
            while (true)
            {

                double current = drPort.getPosition().x;
                double target = targetPoint.x;
                int xSpeed = (int)(150 * (current - target));

                double error1 = Math.Abs(current - target);

                current = drPort.getPosition().y;
                target = targetPoint.y;
                int ySpeed = (int)(-150 * (current - target));

                double error2 = Math.Abs(current - target);

                current = drPort.getPosition().w;
                target = targetPoint.w;
                int wSpeed = (int)(-100 * (current - target));

                double error3 = Math.Abs(current - target);

                if (error1 < 0.04 && error2 < 0.02 && error3 < 0.05) { break; }


                if (xSpeed < -50) { xSpeed = -50; }
                if (xSpeed > 50) { xSpeed = 50; }
                if (ySpeed < -100) { ySpeed = -100; }
                if (ySpeed > 100) { ySpeed = 100; }
                if (wSpeed < -10) { wSpeed = -10; }
                if (wSpeed > 10) { wSpeed = 10; }

                conPort.Control_Move_By_Speed(ySpeed, xSpeed, wSpeed);
                System.Threading.Thread.Sleep(100);
            }
        }

        private int getForwardSpeed(int keepSpeed, KeyPoint targetPoint, IUrgPort urgPort, IDrPort drPort)
        {
            // 取点
            List<CONFIG.URG_POINT> pointsH = getUrgPoint(85, 95, urgPort);

            // 数量不够
            if (pointsH.Count == 0) { return keepSpeed; }

            // 换成绝对距离
            for (int i = 0; i < pointsH.Count; i++)
            { CONFIG.URG_POINT point = pointsH[i]; point.y = Math.Abs(point.y); pointsH[i] = point; }

            // 距离限速
            double minH = double.MaxValue;
            for (int i = 0; i < pointsH.Count; i++)
            { if (minH > pointsH[i].y) { minH = pointsH[i].y; } }
            if (minH < 200) { config.AchieveTarget = true; return 0; }

            // 退出条件有变
            //double current = drPort.getPosition().y * 1000;
            //double target = targetPoint.y * 1000;
            //if (Math.Abs(current - target) < 10) { config.AchieveTarget = true; }
            //int ForwardSpeed = (int)PDcontroller(current, target, ref config.PD_F);

            double current = minH;
            double target = 330;
            if (Math.Abs(current - target) < 20) { config.AchieveTarget = true; return 0; }
            int ForwardSpeed = -(int)PDcontroller(current, target, ref config.PD_F);

            // 限速
            if (ForwardSpeed > config.MaxForwardSpeed) { return config.MaxForwardSpeed; }
            if (ForwardSpeed < -config.MaxForwardSpeed) { return -config.MaxForwardSpeed; }
            return ForwardSpeed;
        }
        private int getTranslateSpeed(double keepLeft, IConPort conPort, IUrgPort urgPort, IDrPort drPort)
        {
            // 取数据
            SonicModel sonic = conPort.Measure_Sonic();
            double distanceL = 0, distanceR = 0;

            if (sonic.S[0] < 1000) { distanceL = sonic.S[0]; }
            if (sonic.S[7] < 1000) { if (distanceL == 0) { distanceL = sonic.S[7]; } else { distanceL = Math.Min(distanceL, sonic.S[7]); } }
            if (sonic.S[3] < 1000) { distanceR = sonic.S[3]; }
            if (sonic.S[4] < 1000) { if (distanceR == 0) { distanceR = sonic.S[4]; } else { distanceR = Math.Min(distanceR, sonic.S[4]); } }

            if (distanceL != 0) { distanceL += 225; }
            if (distanceR != 0) { distanceR += 225; }
            if (distanceL == 0) { distanceL = double.MaxValue; }
            if (distanceR == 0) { distanceR = double.MaxValue; }

            // 不适用超声波数据
            distanceL = double.MaxValue;
            distanceR = double.MaxValue;

            // 加点前瞻
            List<CONFIG.URG_POINT> pointsL = getUrgPoint(120, 180, urgPort);
            List<CONFIG.URG_POINT> pointsR = getUrgPoint(0, 60, urgPort);

            double minL = double.MaxValue, minR = double.MaxValue;
            for (int i = 0; i < pointsL.Count; i++)
            {
                double x = Math.Abs(pointsL[i].x);
                if (x < minL) { minL = x; }
            }
            for (int i = 0; i < pointsR.Count; i++)
            {
                double x = Math.Abs(pointsR[i].x);
                if (x < minR) { minR = x; }
            }

            distanceL = Math.Min(distanceL, minL);
            distanceR = Math.Min(distanceR, minR);

            if (distanceL == 0 && distanceR == 0) { return 0; }

            // 判断通道宽度
            double AisleWidth = 800;
            

            // 获取控制
            int TranslateSpeed = 0;

            // 最短距离模式
            double acceptDistance = 320;
            bool acceptL =  distanceL <acceptDistance;
            bool acceptR = distanceR < acceptDistance;

            if (acceptL && acceptR)
            {
                double current = distanceL;
                double target = (distanceL + distanceR) / 2;

                TranslateSpeed = (int)PDcontroller(current, target, ref config.PD_T);
            }
            if (acceptL &&! acceptR)
            {
                double current = distanceL;
                double target = (distanceL < AisleWidth) ? (distanceL + distanceR) : 180; //Math.Abs(keepLeft);

                TranslateSpeed = (int)PDcontroller(current, target, ref config.PD_T);
            }
            if (!acceptL && acceptR)
            {
                double current = distanceR;
                double target = (distanceL < AisleWidth) ? (distanceL + distanceR) : 180; //Math.Abs(keepLeft);

                TranslateSpeed = -(int)PDcontroller(current, target, ref config.PD_T);
            }

            // 限速
            if (TranslateSpeed > config.MaxTranslateSpeed) { return config.MaxTranslateSpeed; }
            if (TranslateSpeed < -config.MaxTranslateSpeed) { return -config.MaxTranslateSpeed; }
            if (acceptL || acceptR) { return TranslateSpeed; }

            // 数据能否使用
            if (0 < distanceL && distanceL < AisleWidth) { acceptL = true; }
            if (0 < distanceR && distanceR < AisleWidth) { acceptR = true; }

            // 数据无效模式
            if (!acceptL && !acceptR) { return 0; }

            if (!acceptL)
            {

                double current = distanceR;
                double target = AisleWidth/2;

                TranslateSpeed = -(int)PDcontroller(current, target, ref config.PD_T);
                if (TranslateSpeed > config.MaxTranslateSpeed) { return config.MaxTranslateSpeed; }
                if (TranslateSpeed < -config.MaxTranslateSpeed) { return -config.MaxTranslateSpeed; }
                return TranslateSpeed;
            }

            if (!acceptR) {
                double current = distanceL;
                double target = AisleWidth / 2;

                TranslateSpeed = (int)PDcontroller(current, target, ref config.PD_T);
                if (TranslateSpeed > config.MaxTranslateSpeed) { return config.MaxTranslateSpeed; }
                if (TranslateSpeed < -config.MaxTranslateSpeed) { return -config.MaxTranslateSpeed; }
                return TranslateSpeed;
            }

            // 左中右模式
            if (keepLeft == 0)
            {
                double current = distanceL;
                double target = (distanceL + distanceR) / 2;
                TranslateSpeed = (int)PDcontroller(current, target, ref config.PD_T);
            }
            if (keepLeft > 0)
            {
                double current = distanceL;
                double target = keepLeft;
                TranslateSpeed = (int)PDcontroller(current, target, ref config.PD_T);
            }
            if (keepLeft < 0)
            {
                double current = distanceR;
                double target = -keepLeft;
                TranslateSpeed = -(int)PDcontroller(current, target, ref config.PD_T);
            }

            


            // 限速
            if (TranslateSpeed > config.MaxTranslateSpeed) { return config.MaxTranslateSpeed; }
            if (TranslateSpeed < -config.MaxTranslateSpeed) { return -config.MaxTranslateSpeed; }
            return TranslateSpeed;
        }
        private int getRotateSpeed(IConPort conPort, IUrgPort urgPort, IDrPort drPort)
        {
            List<CONFIG.URG_POINT> pointsH = getUrgPoint(85, 95, urgPort);
            double minH = double.MaxValue;
            for (int i = 0; i < pointsH.Count; i++)
            {
                double y = pointsH[i].y;
                if (y < minH) { minH = y; }
            }
            if (minH < 1000) { return 0; }

            // 取点
            List<CONFIG.URG_POINT> pointsL = getUrgPoint(120, 180, urgPort);
            List<CONFIG.URG_POINT> pointsR = getUrgPoint(0, 60, urgPort);
            
            // 交换并取绝对坐标
            for (int i = 0; i < pointsL.Count; i++)
            {
                CONFIG.URG_POINT point = pointsL[i];

                double tempx = Math.Abs(point.x);
                double tempy = Math.Abs(point.y);
                point.x = tempy; point.y = tempx; pointsL[i] = point;
            }
            for (int i = 0; i < pointsR.Count; i++)
            {
                CONFIG.URG_POINT point = pointsR[i];

                double tempx = Math.Abs(point.x);
                double tempy = Math.Abs(point.y);
                point.x = tempy; point.y = tempx; pointsR[i] = point;
            }

            // 一米之内
            for (int i = pointsL.Count - 1; i >= 0; i--)
            {
                if (pointsL[i].y > 1000) { pointsL.RemoveAt(i); }
            }
            for (int i = pointsR.Count - 1; i >= 0; i--)
            {
                if (pointsR[i].y > 1000) { pointsR.RemoveAt(i); }
            }


            // 拟合左右两边障碍物信息
            pointsL = SortPoints(pointsL);
            pointsR = SortPoints(pointsR);
            pointsL = getFitPoints(pointsL);
            pointsR = getFitPoints(pointsR);
            double[] KAB_L = getFitLine(pointsL);
            double[] KAB_R = getFitLine(pointsR);

            // 点数量不够
            bool acceptL = pointsL.Count > 10;
            bool acceptR = pointsR.Count > 10;
            if (!acceptL && !acceptR) { return 0; }

            // 控制策略
            int RotateSpeed = 0;

            if (acceptL && acceptR)
            {
                double current = 0;
                double target = (KAB_L[1] - KAB_R[1]) / 2;

                RotateSpeed = (int)PDcontroller(current, target, ref config.PD_R);
            }
            if (acceptL && !acceptR)
            {
                double current = 0;
                double target = KAB_L[1];

                RotateSpeed = (int)PDcontroller(current, target, ref config.PD_R);
            }
            if (!acceptL && acceptR)
            {
                double current = 0;
                double target = KAB_R[1];

                RotateSpeed = -(int)PDcontroller(current, target, ref config.PD_R);
            }

            // 判断是否允许旋转
            double permitRotateDistance = 100; //100
            SonicModel sonic = conPort.Measure_Sonic();
            //foreach (int s in sonic.S) { if (s < permitRotateDistance) { return 0; } }
            
            // 限速
            if (RotateSpeed > config.MaxRotateSpeed) { return config.MaxRotateSpeed; }
            if (RotateSpeed < -config.MaxRotateSpeed) { return -config.MaxRotateSpeed; }
            return RotateSpeed;
        }

        private static List<CONFIG.URG_POINT> SortPoints(List<CONFIG.URG_POINT> points)
        {
            // 点的数量不够，直接返回。
            if (points.Count <= 3) { return points; }

            // 距离排序
            for (int i = 1; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count - i; j++)
                {
                    if (points[j].y <= points[j + 1].y) { continue; }

                    CONFIG.URG_POINT temp = new CONFIG.URG_POINT();
                    temp = points[j];
                    points[j] = points[j + 1];
                    points[j + 1] = temp;
                }
            }

            // 选取距离
            int indexofcut = points.Count;
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (points[i + 1].y - points[i].y > 200) { indexofcut = i + 1; break; }
            }
            points.RemoveRange(indexofcut, points.Count - indexofcut);

            // 距离跨度要求
            double xMax = double.MinValue, xMin = double.MaxValue;

            for (int i = 0; i < points.Count; i++)
            {
                double x = points[i].x;
                if (x > xMax) { xMax = x; }
                if (x < xMin) { xMin = x; }
            }
            if (xMax - xMin < 200) { return new List<CONFIG.URG_POINT>(); }

            return points;
        }
        private static List<CONFIG.URG_POINT> getFitPoints(List<CONFIG.URG_POINT> points)
        {
            // 不能这样选点，太抖了
            return points;

            // 点的数量不够，直接返回。
            if (points.Count <= 3) { return points; }

            // 最近的点作为基准点
            double x0 = 0.0, y0 = double.MaxValue;
            int closest = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].y > y0) { continue; }

                x0 = points[i].x;
                y0 = points[i].y;
                closest = i;
            }

            // 其余点相对角度
            List<double> angles = new List<double>();
            double MaxAngle = 0;
            double MinAngle = double.MaxValue;

            for (int i = 0; i < points.Count; i++)
            {
                if (i == closest) { angles.Add(0.0); continue; }

                double angle = Math.Atan((points[i].y - y0) / (points[i].x - x0));
                if (points[i].x < x0) { angle += 180; }

                angles.Add(angle);
                if (angle > MaxAngle) { MaxAngle = angle; }
                if (angle < MinAngle) { MinAngle = angle; }
            }

            // 根据最近点的相对小车距离决定选取那个角度
            double targetAngle = x0 < 400 ? MaxAngle : MinAngle;
            angles[closest] = targetAngle;

            // 取出斜率符合的点
            List<CONFIG.URG_POINT> fitpoints = new List<CONFIG.URG_POINT>();
            for (int i = 1; i < angles.Count; i++)
            {
                if (Math.Abs(angles[i] - targetAngle) < 1.0) { fitpoints.Add(points[i]); continue; }
                if (Math.Abs(angles[i] + targetAngle - 180) < 1.0) { fitpoints.Add(points[i]); }
            }

            // 返回
            return fitpoints;
        }
        private static double[] getFitLine(List<CONFIG.URG_POINT> points)
        {
            // 点数量不够
            if (points.Count <= 3) { return new double[3] { 0, 0, 0 }; }

            // 拟合直线
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
            if (denominator == 0) { denominator = 0.000000001; }

            // 计算斜率和截距
            double UrgK = (N * sumXY - sumX * sumY) / denominator;
            double UrgB = (sumXX * sumY - sumX * sumXY) / denominator;

            double UrgA = Math.Atan(UrgK) * 180 / Math.PI;
            if (Math.Abs(UrgA) > 60) { return new double[3] { 0, 0, 0 }; }

            return new double[3] { UrgK, UrgA, UrgB };
        }

        private List<CONFIG.URG_POINT> getUrgPoint(double angleBG, double angleED, IUrgPort urgPort)
        {
            List < CONFIG.URG_POINT > points = new List<CONFIG.URG_POINT>();
            //if (!urgPort.IsOpen) { return points; }

            UrgModel urgModel = urgPort.getUrgData();
            while (urgModel.Distance == null || urgModel.Distance.Count == 0) { urgModel = urgPort.getUrgData(); }

            int BG = (int)((angleBG - -30) / (360.0/1024.0));
            int ED = (int)((angleED - -30) / (360.0/1024.0));
            if (urgModel.Distance.Count < ED) { return points; }
            
            for (int i = BG; i < ED; i++)
            {
                if (urgModel.Distance[i] == 0) { continue; }

                CONFIG.URG_POINT point = new CONFIG.URG_POINT();
                point.d = urgModel.Distance[i];

                double angle = -30.0 + i * 360.0/1024.0;

                point.a = angle;
                point.x = point.d * Math.Cos(angle * Math.PI / 180);
                point.y = point.d * Math.Sin(angle * Math.PI / 180);

                points.Add(point);
            }

            return points;
        }
        private double PDcontroller(double current, double target, ref CONFIG.PD_PARAMETER PD)
        {
            PD.Error2 = PD.Error1;
            PD.Error1 = PD.Error0;
            PD.Error0 = current - target;

            double pControl = PD.Kp * PD.Error0;
            double dControl = PD.Kd * (PD.Error0 - PD.Error1);

            return -(pControl + dControl);
        }
    }
}
