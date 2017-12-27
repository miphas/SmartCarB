using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartCar {
    public partial class Form1 : Form {
        public Form1() {

            InitializeComponent();

            initMForms();
        }

        /// <summary>
        /// 初始化三个窗口
        /// </summary>
        public static Form[] MForms;
        public void initMForms() {
            MForms = new Form[] {
                new Form_Guide(),
                new Form_Path(),
                new Form_Param()
            };
            // 设置窗口为嵌入页面
            for (int i = 0; i < MForms.Length; ++i) {
                MForms[i].TopLevel = false;
                MForms[i].Location = new Point(0, 0);
            }
            // 显示第一个页面
            this.panelShow.Controls.Add(MForms[0]);
            for (int i = 0; i < MForms.Length; ++i) {
                MForms[i].Visible = true;
            }
            
            ThreadManager.start();
            DisplayMethod.curState = DisplayMethod.displayItem.RefGuide;
        }
        /// <summary>
        /// 显示自动导航窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuide_Click(object sender, EventArgs e) {
            this.panelShow.Controls.Clear();
            this.panelShow.Controls.Add(MForms[0]);
            DisplayMethod.curState = DisplayMethod.displayItem.RefGuide;
        }
        /// <summary>
        /// 显示路径编辑窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPath_Click(object sender, EventArgs e) {
            this.panelShow.Controls.Clear();
            this.panelShow.Controls.Add(MForms[1]);
            DisplayMethod.curState = DisplayMethod.displayItem.RefPath;
        }
        /// <summary>
        /// 显示参数编辑窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParam_Click(object sender, EventArgs e) {
            this.panelShow.Controls.Clear();
            this.panelShow.Controls.Add(MForms[2]);
            DisplayMethod.curState = DisplayMethod.displayItem.DoNothing;
        }

        /// <summary>
        /// 窗口大小改变时，设定子窗口大小变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelShow_Resize(object sender, EventArgs e) {
            for (int i = 0; MForms != null && i < MForms.Length; ++i) {
                MForms[i].Width = this.panelShow.Width;
                MForms[i].Height = this.panelShow.Height;
            }
        }

        /// <summary>
        /// 关闭窗口是关闭所有串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public static bool closing = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            closing = true;
            PortManager.closeAllPort();

            ControlMethod.curState = ControlMethod.ctrlItem.DoNothing;
            DisplayMethod.curState = DisplayMethod.displayItem.DoNothing;
            ThreadManager.end();
        }
        


    }
}
