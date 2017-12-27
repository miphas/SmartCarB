using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    interface ICorrectPosition
    {
        /// <summary>
        /// 得到当前点的斜率和截距
        /// </summary>
        /// <param name="urgPort">激光雷达口</param>
        /// <returns></returns>
        KeyPoint getCurrentKB(UrgPort urgPort);

        /// <summary>
        /// 开始校准
        /// </summary>
        /// <param name="conPort">控制口</param>
        /// <param name="urgPort">激光雷达口</param>
        /// <param name="keyPoint">关键点</param>
        void Start(ConPort conPort, UrgPort urgPort, KeyPoint keyPoint);
    }
}
