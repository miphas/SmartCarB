using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public interface IDrPort {
        /// <summary>
        /// 获取当前点
        /// </summary>
        KeyPoint getPosition();

        /// <summary>
        /// 设置当前点
        /// </summary>
        void setPosition(KeyPoint keyPoint);
        void setPosition(double x, double y, double w);
        
    }
}
