using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SmartCar {
    public class DirUtil : IDirUtil {
        /// <summary>
        /// 创建文件路径
        /// </summary>
        /// <param name="path">需要创建的文件路径</param>
        /// <returns>是否创建成功</returns>
        public bool createDir(string path) {
            // 已存在则返回成功
            if (Directory.Exists(path)) {
                return true;
            }
            // 尝试创建目录
            try {
                Directory.CreateDirectory(path);
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除文件路径
        /// </summary>
        /// <param name="path">需要删除的文件路径</param>
        /// <returns>是否删除成功</returns>
        public bool deleteDir(string path) {
            // 目录不存在则返回
            if (!Directory.Exists(path)) {
                return false;
            }
            // 尝试删除目录
            try {
                Directory.Delete(path);
            }
            catch (Exception) {
                return false;
            }
            // 返回删除成功
            return true;
        }

        /// <summary>
        /// 返回目录存在情况
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool existDir(string path) {
            return Directory.Exists(path);
        }


        private String currentPath = null;
        /// <summary>
        /// 返回exe当前所处文件路径
        /// </summary>
        /// <returns></returns>
        public String getCurrentPath() {
            // 没有则创建
            if (this.currentPath == null) {
                // currentDirectory目录不包含最后面反斜杠
                this.currentPath = Environment.CurrentDirectory;
                if (!this.currentPath.EndsWith("\\")) {
                    this.currentPath += "\\";
                }
            }
            return this.currentPath; 
        }


        /// <summary>
        /// 创建指定文件（未实现）
        /// </summary>
        /// <param name="pathAndFile"></param>
        /// <returns></returns>
        public bool createFile(string pathAndFile) {
            return false;
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="pathAndFile"></param>
        /// <returns></returns>
        public bool deleteFile(string pathAndFile) {
            try {
                File.Delete(pathAndFile);
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="pathAndFile">路径+文件名</param>
        /// <returns></returns>
        public bool existFile(string pathAndFile) {
            return File.Exists(pathAndFile);
        }
    }
}
