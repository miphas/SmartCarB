using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    /// <summary>
    /// 车体信息类
    /// </summary>
    public class CarInfo : Info
    {
        /// <summary>
        /// 车体大小信息
        /// </summary>
        public double CarH { get; set; }
        public double CarW { get; set; }
        /// <summary>
        /// 雷达位置信息
        /// </summary>
        public double RadT { get; set; }
        public double RadL { get; set; }

        public KeyPoint LastPoint { get; set; }

        // 存储上述属性名
        public static String[] FieldName = new String[] {
            "CarH", "CarW", "RadT", "RadL"
        };
        public int[] FieldId = new int[] {
            (int)SPAM.paramE.CarH, (int)SPAM.paramE.CarW, 
            (int)SPAM.paramE.RadTop, (int)SPAM.paramE.RadLeft
        };

        public CarInfo()
        {
            this.LastPoint = new KeyPoint();
        }

        /// <summary>
        /// 更新属性值
        /// </summary>
        public override void updateInfo() {
            for (int i = 0; i < FieldName.Length; ++i) {
                ValSet.SetModelValue(FieldName[i], DataArea.infoModel.Data[FieldId[i]], this);
            }
        }

        public static KeyPoint getRadarPosition(KeyPoint carPosi) {
            double BoardHeight = 1.02;
            double BoardWidth = 0.44;

            double RadarTop = -0.03;
            double RadarLef = 0.22;
            KeyPoint radarPos = new KeyPoint() {
                x = carPosi.x + Math.Cos(carPosi.w + Math.PI / 2) * (BoardHeight / 2 - RadarTop),
                y = carPosi.y + Math.Sin(carPosi.w + Math.PI / 2) * (BoardHeight / 2 - RadarTop)
            };
            //Console.WriteLine(radarPos.x + " " + radarPos.y);
            return radarPos;
        }

    }
}
