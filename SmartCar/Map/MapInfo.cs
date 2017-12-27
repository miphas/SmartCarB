using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    public class MapInfo
    {
        public static String[] pItem = new String[] {
            "id", "x", "y", "w", "type", "Can_Adj", "UrgK", "UrgB", "wayType", "disWay", "moveBack"
        };
        public enum pItemE
        {
            id,
            x,
            y,
            w,
            type,
            Can_Adj,
            UrgK,
            UrgB,
            wayType,
            disWay,
            moveBack
        }

        public static String root = "Information";
        public static String typeP = "point";
        public static String point = "roadpoint";

        /// <summary>
        /// 关键点类型
        /// </summary>
        public static String[] pointType = new string[] {
            "普通关键点", "激光雷达关键点"
        };
        public enum pointTypeE
        {
            General,
            Radar
        }


        public static String typeS = "seg";
        public static String nameS = "segment";
        public static String detLS = "lp";
        public static String detRS = "rp";

        public enum sItemE
        {
            x,
            y
        }
        public static String[] sItem = new String[] {
            "x", "y"
        };


    }
}
