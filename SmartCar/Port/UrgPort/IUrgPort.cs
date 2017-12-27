using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public interface IUrgPort {
        /// <summary>
        /// 获取上一时刻完整激光雷达数据
        /// </summary>
        UrgModel getUrgData();

    }
}
