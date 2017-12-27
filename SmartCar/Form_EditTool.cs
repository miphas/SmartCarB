using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartCar {
    public partial class Form_EditTool : Form {
        public Form_EditTool() {
            InitializeComponent();
        }

        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键
        private void toolStrip1_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void toolStrip1_MouseMove(object sender, MouseEventArgs e) {
            if (leftFlag) {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void toolStrip1_MouseUp(object sender, MouseEventArgs e) {
            if (leftFlag) {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
    }
}
