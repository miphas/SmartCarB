using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    /// <summary>
    /// 串口信息类 用以保存串口等相关信息
    /// </summary>
    public class PortInfo: Info {
        /// <summary>
        /// 控制串口
        /// </summary>
        public String ConPortName { get; set;}
        public int ConPortRate { get; set; }
        /// <summary>
        /// 编码器串口
        /// </summary>
        public String DrPortName { get; set; }
        public int DrPortRate { get; set; }
        /// <summary>
        /// 激光雷达串口
        /// </summary>
        public String UrgPortName { get; set; }
        public int UrgPortRate { get; set; }

        // 存储上述属性名
        public static String[] FieldName = new String[] {
            "ConPortName", "ConPortRate", "DrPortName", "DrPortRate", "UrgPortName", "UrgPortRate"
        };
        public int[] FieldId = new int[] {
            (int)SPAM.paramE.ConPortName, (int)SPAM.paramE.ConPortRate, 
            (int)SPAM.paramE.DrPortName, (int)SPAM.paramE.DrPortRate, 
            (int)SPAM.paramE.UrgPortName, (int)SPAM.paramE.UrgPortRate
        };

        /// <summary>
        /// 更新属性值
        /// </summary>
        public override void updateInfo() {
            for (int i = 0; i < FieldName.Length; ++i) {
                ValSet.SetModelValue(FieldName[i], DataArea.infoModel.Data[FieldId[i]], this);
            }
        }


    }
}
