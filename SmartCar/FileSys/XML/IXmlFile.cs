using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartCar {
    /// <summary>
    /// XML文件操作接口
    /// </summary>
    public interface IXmlFile {
        /// <summary>
        /// 创建XML文件
        /// </summary>
        bool creaXmlFile(String pathAndName);
        /// <summary>
        /// 加载XML文件
        /// </summary>
        bool loadXmlFile(String pathAndName);


        /// <summary>
        /// 读取XML数据
        /// </summary>
        XmlNode readData();
        /// <summary>
        /// 保存XML数据
        /// </summary>
        bool saveData();
    }
}
