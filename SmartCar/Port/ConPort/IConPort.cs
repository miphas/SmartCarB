using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    interface IConPort {
        
        /// <summary>
        /// 设置急停
        /// </summary>
        void Control_Stop();

        /// <summary>
        /// 继续行进
        /// </summary>
        void Control_Continue();

        /// <summary>
        /// 以偏角以及速度来控制向前行进
        /// </summary>
        /// <param name="forwardSpeed">行进速度 单位：mm</param>
        /// <param name="direction">行进方向 单位：度</param>
        /// <param name="rotateSpeed">旋转速度 单位：0.01弧度</param>
        /// <returns>指令是否发送成功</returns>
        bool Control_Move_By_Direct(int forwardSpeed, int direction, int rotateSpeed);
        
        /// <summary>
        /// 以向前速度以及向左速度控制行进
        /// </summary>
        /// <param name="forwardSpeed">向前速度 单位：mm</param>
        /// <param name="leftSpeed">向左速度 单位：mm</param>
        /// <param name="rotateSpeed">旋转速度 单位：0.01弧度</param>
        /// <returns>指令发送是否成功</returns>
        bool Control_Move_By_Speed(int forwardSpeed, int leftSpeed, int rotateSpeed);

        /// <summary>
        /// 测量超声波数据
        /// </summary>
        /// <returns>返回上一时刻测量值</returns>
        SonicModel Measure_Sonic();
    }
}
