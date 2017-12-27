using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    interface IModel {
        /// <summary>
        /// 从数据更新UI界面
        /// </summary>
        void updateDataFromUI();
        /// <summary>
        /// 从UI界面更新数据
        /// </summary>
        void updateUIFromData();
    }
}
