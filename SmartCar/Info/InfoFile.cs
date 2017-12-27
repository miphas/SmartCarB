using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartCar {
    public class InfoFile {
        private XmlFile file = new XmlFile();
        private DirUtil util = new DirUtil();
        private XmlNode node;
        private String filename;

        public InfoFile() {
            // 获取配置文件路径
            filename = util.getCurrentPath() + "info.xml";
            // 创建或加载配置文件
            if (util.existFile(filename)) {
                file.loadXmlFile(filename);
            }
            else {
                file.creaXmlFile(filename);
                initInfoFile();
            }
        }

        /// <summary>
        /// 将配置文件信息保存到参数模型中
        /// </summary>
        /// <param name="InfoModel"></param>
        public void readNodeData(StrModel infoModel) {
            node = file.readData();
            foreach (XmlNode subNode in node.ChildNodes) {
                int index = SearchUtil.getItemIndex(SPAM.paramName, subNode.Name);
                if (index != -1) {
                    infoModel.Data[index] = subNode.InnerText;
                }
            }
        }
        /// <summary>
        /// 将界面信息保存到文件
        /// </summary>
        /// <param name="InfoModel"></param>
        public bool saveNodeData(StrModel infoModel) {
            if (node == null) {
                node = file.readData();
            }
            foreach (XmlNode subNode in node.ChildNodes) {
                int index = SearchUtil.getItemIndex(SPAM.paramName, subNode.Name);
                if (index != -1) {
                    subNode.InnerText = infoModel.Data[index];
                }
            }
            return file.saveData();
        }

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        private void initInfoFile() {
            node = file.readData();
            if (node.ChildNodes.Count != 0) {
                return;
            }
            for (int i = 0; i < SPAM.paramName.Length; ++i) {
                XmlNode newNode = file.xmlFile.CreateElement(SPAM.paramName[i]);
                newNode.InnerText = SPAM.paramDef[i];
                node.AppendChild(newNode);
            }
            file.saveData();
        }

    }
}
