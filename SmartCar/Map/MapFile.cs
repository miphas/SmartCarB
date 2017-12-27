using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

namespace SmartCar
{
    public class MapFile
    {
        private XmlFile file = new XmlFile();
        private DirUtil util = new DirUtil();
        private XmlNode node;
        private String filename;

        /// <summary>
        /// 构造地图文件
        /// </summary>
        public MapFile(String filename)
        {
            this.filename = filename;
            this.file.Root = MapInfo.root;
            if (util.existFile(filename)) {
                file.loadXmlFile(filename);
            }
            else {
                file.creaXmlFile(filename);
            }
        }

        /// <summary>
        /// 读取路径节点信息
        /// </summary>
        /// <param name="mapModel"></param>
        public void readNodeData(MapModel mapModel)
        {
            // 清除原路径信息
            mapModel.Points.Clear();
            // 读取对应节点
            node = file.readData();
            node = node.SelectSingleNode("/" + MapInfo.root + "/" + MapInfo.typeP);
            for (int i = 0; i < node.ChildNodes.Count; ++i) {
                int id = i + 1;
                XmlNode subNode = node.ChildNodes[i];
                // 使用反射装配属性值
                KeyPoint p = new KeyPoint();
                for (int j = 0; j < subNode.ChildNodes.Count; ++j) {
                    XmlNode pointNode = subNode.ChildNodes[j];
                    // 剔除不存在属性
                    if (SearchUtil.getItemIndex(MapInfo.pItem, pointNode.Name) == -1) {
                        continue;
                    }
                    // 装配属性值
                    ValSet.SetModelValue(pointNode.Name, pointNode.InnerText, p);
                }
                mapModel.Points.Add(p);
            }
            //
            // 读取对应的切割线段信息
            //
            node = node.SelectSingleNode("/" + MapInfo.root + "/" + MapInfo.typeS);
            for (int i = 0; node != null && i < node.ChildNodes.Count; ++i) {
                mapModel.Segments.Add(readSegNode(node.ChildNodes[i]));
            }
        }

        /// <summary>
        /// 保存路径节点信息
        /// </summary>
        /// <param name="mapModel"></param>
        public void writeNodeData(MapModel mapModel)
        {
            node = file.readData();
            node.RemoveAll();
            //
            // 添加关键点部分
            //
            XmlNode pNode = file.xmlFile.CreateElement(MapInfo.typeP);
            for (int i = 0; i < mapModel.Points.Count; ++i) {
                int pid = i + 1;
                KeyPoint point = mapModel.Points[i];
                // 添加关键点
                XmlNode tNode = file.xmlFile.CreateElement(MapInfo.point + pid.ToString());
                for (int j = 0; j < MapInfo.pItem.Length; ++j) {
                    String val = ValSet.GetModelValue(MapInfo.pItem[j], point);
                    // 添加属性节点
                    XmlNode attr = file.xmlFile.CreateElement(MapInfo.pItem[j]);
                    attr.InnerText = (val == null ? "0" : val);
                    tNode.AppendChild(attr);
                }
                pNode.AppendChild(tNode);
            }
            node.AppendChild(pNode);
            //
            // 添加Segment部分
            //
            XmlNode sNode = file.xmlFile.CreateElement(MapInfo.typeS);
            for (int i = 0; i < mapModel.Segments.Count; ++i) {
                sNode.AppendChild(getSegNode(file.xmlFile, mapModel.Segments[i], i + 1));
            }
            node.AppendChild(sNode);
            // 保存文件信息
            file.saveData();
        }

        /// <summary>
        /// 写入segment节点信息
        /// </summary>
        private XmlNode getSegNode(XmlDocument file, Segment seg, int id)
        {
            // 创建segment节点
            XmlNode ans = file.CreateElement(MapInfo.nameS + id.ToString());
            // 添加左侧点
            for (int i = 0; i < seg.LeftP.Count; ++i) {
                XmlNode p = file.CreateElement(MapInfo.detLS);
                p.AppendChild(getValNode(file, MapInfo.sItem[0], seg.LeftP[i].x.ToString()));
                p.AppendChild(getValNode(file, MapInfo.sItem[1], seg.LeftP[i].y.ToString()));
                ans.AppendChild(p);
            }
            // 添加右侧点
            for (int i = 0; i < seg.RightP.Count; ++i) {
                XmlNode p = file.CreateElement(MapInfo.detRS);
                p.AppendChild(getValNode(file, MapInfo.sItem[0], seg.RightP[i].x.ToString()));
                p.AppendChild(getValNode(file, MapInfo.sItem[1], seg.RightP[i].y.ToString()));
                ans.AppendChild(p);
            }
            return ans;
        }
        /// <summary>
        /// 读取segment节点信息
        /// </summary>
        private Segment readSegNode(XmlNode segNode)
        {
            Segment ans = new Segment();
            for (int i = 0; i < segNode.ChildNodes.Count; ++i) {
                XmlNode sub = segNode.ChildNodes[i];
                // 填充节点信息
                SimPoint simP = new SimPoint();
                for (int j = 0; j < sub.ChildNodes.Count; ++j) {
                    ValSet.SetModelValue(sub.ChildNodes[j].Name, sub.ChildNodes[j].InnerText, simP);
                }
                // 添加至左侧或者右侧
                if (sub.Name.Equals(MapInfo.detLS)) {
                    ans.LeftP.Add(simP);
                }
                else {
                    ans.RightP.Add(simP);
                }
            }
            return ans;
        }

        /// <summary>
        /// 获取带值的节点
        /// </summary>
        private XmlNode getValNode(XmlDocument file, String name, String value)
        {
            XmlNode node = file.CreateElement(name);
            node.InnerText = value;
            return node;
        }

    }
}
