using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartCar {
    public class StrModel: IModel {
        /// <summary>
        /// 存放数据以及控件
        /// </summary>
        String[] data;
        Control[] ctrl;
        Dictionary<Control, int> ctrlTag = new Dictionary<Control, int>();

        /// <summary>
        /// 数据变换委托
        /// </summary>
        public delegate void dataChangedDel();
        public dataChangedDel dataChanged;
        /// <summary>
        /// 数据域
        /// </summary>
        public String[] Data {
            get { return data; }
            set { data = value; }
        }
        public Dictionary<Control, int> CtrlTag {
            get { return ctrlTag; }
        }

        /// <summary>
        /// 字符串模型构造函数
        /// </summary>
        /// <param name="ctrls"></param>
        public StrModel(params Control[] ctrls) {
            // 创建数据域以及控件域
            this.data = new String[ctrls.Length];
            this.ctrl = new Control[ctrls.Length];
            // 存储控件
            for (int i = 0; i < ctrls.Length; ++i) {
                this.ctrl[i] = ctrls[i];
                this.ctrlTag.Add(this.ctrl[i], i);
            }
        }

        /// <summary>
        /// 从UI更新数据
        /// </summary>
        public virtual void updateDataFromUI() {
            for (int i = 0; i < ctrl.Length; ++i) {
                this.data[i] = this.ctrl[i].Text;
            }
            if (dataChanged != null) {
                dataChanged();
            }
        }
        /// <summary>
        /// 从数据更新UI
        /// </summary>
        public virtual void updateUIFromData() {
            for (int i = 0; i < data.Length; ++i) {
                this.ctrl[i].Text = this.data[i];
            }
        }

    }
}
