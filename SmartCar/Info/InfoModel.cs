using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartCar {
    public class InfoModel: StrModel {
        /// <summary>
        /// 保存对应关系
        /// </summary>
        Dictionary<Control, TrackBar> dic = new Dictionary<Control, TrackBar>();

        public InfoModel(params Control[] ctrls) : base(ctrls) {
            // 绑定数据变换事件
            this.dataChanged = new dataChangedDel(this.paramDataChanged);
        }

        /// <summary>
        /// 添加tracBar控件与原控件的对应关系
        /// </summary>
        /// <param name="tracVal">先textbox空间，然后tracBar控件，两个一组</param>
        public bool addTracBar(params Control[] tracVal) {
            try {
                for (int i = 0; i < tracVal.Length; i += 2) {
                    dic.Add(tracVal[i], (TrackBar)tracVal[i + 1]);
                }
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 重写更新UI控件方法
        /// </summary>
        public override void updateUIFromData(){
            base.updateUIFromData();
            foreach (var item in dic) {
                item.Value.Value = int.Parse(Data[CtrlTag[item.Key]]);
            }
        }

        /// <summary>
        /// 跟随数据变化 更新各类信息
        /// </summary>
        private void paramDataChanged() {
            InfoManager.updateAllInfo();
        }

    }
}
