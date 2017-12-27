using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    /// <summary>
    /// 行进信息类
    /// </summary>
    public class MoveInfo : Info
    {
        /// <summary>
        /// 前进速度
        /// </summary>
        public int MaxFront { get; set; }
        /// <summary>
        /// 左侧速度
        /// </summary>
        public int MaxSide { get; set; }
        /// <summary>
        /// 旋转速度
        /// </summary>
        public int MaxRot { get; set; }

        // 存储上述属性名
        public static String[] FieldName = new String[] {
            "MaxFront", "MaxSide", "MaxRot"
        };
        public int[] FieldId = new int[] {
            (int)SPAM.paramE.MaxFront, (int)SPAM.paramE.MaxSide, 
            (int)SPAM.paramE.MaxRotate
        };
        /// <summary>
        /// 更新属性值
        /// </summary>
        public override void updateInfo() {
            for (int i = 0; i < FieldName.Length; ++i) {
                ValSet.SetModelValue(FieldName[i], DataArea.infoModel.Data[FieldId[i]], this);
            }
        }

        // Defination Direction
        public enum Direction
        {
            Front, Left, Right, Back
        }
        // get the direction of the single path
        public static Direction getDirect(KeyPoint staP, KeyPoint endP){
            double dx = endP.x - staP.x;
            double dy = endP.y - staP.y;
            return (Math.Abs(dy) > Math.Abs(dx)) ?
                (dy > 0 ? Direction.Front : Direction.Back) :
                (dx > 0 ? Direction.Right : Direction.Left);
        }
        
    }
}
