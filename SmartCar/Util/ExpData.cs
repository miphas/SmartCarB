using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SmartCar.Util
{
    public class ExpData
    {
        private static FileStream fs;
        private static StreamWriter sw;
        public static void createFile()
        {
            fs = new FileStream("data.txt", FileMode.Create);
            sw = new StreamWriter(fs);
        }
        public static void writeData(List<long> radarData, double x, double y, double w)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append(addRadarData(radarData));
            sb.Append(",");
            sb.Append(addPosData(x, y, w));
            sb.Append("}");
            sw.WriteLine(sb.ToString());
        }
        public static void closeFile()
        {
            sw.Close();
            fs.Close();
        }

        private static String addRadarData(List<long> radarData)
        {
            StringBuilder sb = new StringBuilder();
            // 添加长度
            sb.Append("\"len\":");
            sb.Append(radarData.Count + ",");
            // 添加距离信息
            sb.Append("\"dis\":");
            sb.Append("[" + (radarData.Count > 0 ? radarData[0] + "" : ""));
            for (int i = 1; i < radarData.Count; i++)
            {
                sb.Append("," + radarData[i]);
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static String addPosData(double x, double y, double w)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"pos\":");
            sb.Append("[");
            sb.Append(x + "," + y + "," + w);
            sb.Append("]");
            return sb.ToString();
        }

    }
}
