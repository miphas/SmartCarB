using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class DataArea {
        
        /// <summary>
        /// 参数文件 参数模型
        /// </summary>
        public static InfoFile infoFile;
        public static InfoModel infoModel;

        /// <summary>
        /// 路径文件 路径模型
        /// </summary>
        public static MapFile mapFile;
        public static MapModel mapModel;

        /// <summary>
        /// 画图信息
        /// </summary>
        public static DrawFormat drawFormat = new DrawFormat();

    }
}
