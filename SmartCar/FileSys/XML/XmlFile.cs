using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartCar {
    public class XmlFile : IXmlFile{

        private XmlDocument file;
        private String pathAndName;
        private String root = "root";

        public XmlDocument xmlFile {
            get { return file; }
        }
        public String Root {
            get { return root; }
            set { root = value; }
        }
        /// <summary>
        /// 创建XML文件
        /// </summary>
        /// <param name="pathAndName">路径+文件名</param>
        /// <returns>创建文件是否成功</returns>
        public bool creaXmlFile(string pathAndName) {
            try {
                this.pathAndName = pathAndName;
                file = new XmlDocument();
                // 添加描述、根节点至文档（<?xml version="1.0" encoding="utf-8" ?>）
                file.AppendChild(file.CreateXmlDeclaration("1.0", "utf-8", null));
                file.AppendChild(file.CreateElement(root));
                // 保存文件
                file.Save(pathAndName);
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <param name="pathAndName">路径+文件名</param>
        /// <returns>加载文件是否成功</returns>
        public bool loadXmlFile(string pathAndName) {
            try {
                this.pathAndName = pathAndName;
                file = new XmlDocument();
                file.Load(pathAndName);
            }
            catch (Exception) {
                return false;
            }
            return true;
        }
        

        /// <summary>
        /// 读取XML数据
        /// </summary>
        /// <returns>XmlNode数据节点</returns>
        public XmlNode readData() {
            try {
                XmlNode n =  file.SelectSingleNode("/" + root);
                return n;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// 保存XML文件
        /// </summary>
        /// <returns>是否保存成功</returns>
        public bool saveData() {
            try {
                file.Save(pathAndName);
            }
            catch (Exception) {
                return false;
            }
            return true;
        }
    }
}
