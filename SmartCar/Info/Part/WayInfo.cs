using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    /// <summary>
    /// 通道信息类
    /// </summary>
    public class WayInfo : Info {
        /// <summary>
        /// 通道宽度
        /// </summary>
        public int WayWidth { get; set; }
        /// <summary>
        /// 靠右距离
        /// </summary>
        public int WayRight { get; set; }
        /// <summary>
        /// 靠左距离
        /// </summary>
        public int WayLeft { get; set; }

        // 存储上述属性名
        public static String[] FieldName = new String[] {
            "WayWidth", "WayRight", "WayLeft"
        };
        public int[] FieldId = new int[] {
            (int)SPAM.paramE.WayWidth, (int)SPAM.paramE.WayRight, 
            (int)SPAM.paramE.WayLeft
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
