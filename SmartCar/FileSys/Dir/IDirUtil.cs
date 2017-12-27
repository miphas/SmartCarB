using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    interface IDirUtil {
        //
        // 目录相关操作
        //
        /// <summary>
        /// 创建目录
        /// </summary>
        bool createDir(String path);
        /// <summary>
        /// 删除目录
        /// </summary>
        bool deleteDir(String path);
        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        bool existDir(String path);
        /// <summary>
        /// 获取当前路径
        /// </summary>
        String getCurrentPath();


        //
        // 文件相关操作
        // 
        /// <summary>
        /// 创建文件
        /// </summary>
        bool createFile(String pathAndFile);
        /// <summary>
        /// 删除文件
        /// </summary>
        bool deleteFile(String pathAndFile);
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        bool existFile(String pathAndFile);
    }
}
