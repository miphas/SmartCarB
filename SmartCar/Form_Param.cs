using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartCar {
    public partial class Form_Param : Form {

        public Form_Param() {
            InitializeComponent();

            this.initFormParam();
        }

        public void initFormParam() {
            // 初始化下拉菜单
            this.boxConName.Items.AddRange(SCOM.comName);
            this.boxConRate.Items.AddRange(SCOM.ratName);
            this.boxDrName.Items.AddRange(SCOM.comName);
            this.boxDrRate.Items.AddRange(SCOM.ratName);
            this.boxUrgName.Items.AddRange(SCOM.comName);
            this.boxUrgRate.Items.AddRange(SCOM.ratName);
            // 新建参数模型
            DataArea.infoModel = new InfoModel(
                this.boxConName, this.boxConRate,
                this.boxDrName, this.boxDrRate,
                this.boxUrgName, this.boxUrgRate,
                this.txtFront, this.txtLeft, this.txtRot,
                this.txtWayWidth, this.txtWayLeft, this.txtWayRight,
                this.txtCarW, this.txtCarH,
                this.txtRadTop, this.txtRadLeft
                );
            // 绑定TracBar
            DataArea.infoModel.addTracBar(
                this.txtFront, this.traFront,
                this.txtLeft, this.traLeft,
                this.txtRot, this.traRot
                );
            // 创建参数配置文件并读取数据
            DataArea.infoFile = new InfoFile();
            DataArea.infoFile.readNodeData(DataArea.infoModel);
            InfoManager.init();
            InfoManager.updateAllInfo();
            // 更新界面
            DataArea.infoModel.updateUIFromData();
            // 绑定更新事件
            EventHandler handler = new EventHandler(traFront_ValueChanged);
            this.traFront.ValueChanged += handler;
            this.traLeft.ValueChanged += handler;
            this.traRot.ValueChanged += handler;
        }

        

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveParam_Click(object sender, EventArgs e) {
            DataArea.infoModel.updateDataFromUI();
            if (DataArea.infoFile.saveNodeData(DataArea.infoModel)) {
                MessageBox.Show("配置文件保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("配置文件保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void traFront_ValueChanged(object sender, EventArgs e) {
            this.txtFront.Text = this.traFront.Value.ToString();
            this.txtLeft.Text = this.traLeft.Value.ToString();
            this.txtRot.Text = this.traRot.Value.ToString();
        }

        private void flowLayoutPanel6_Scroll(object sender, ScrollEventArgs e) {
            //Console.WriteLine(this.flowLayoutPanel6.VerticalScroll.Value);
        }
        
        /// <summary>
        /// 设置滚动条相应位置
        /// </summary>
        private void btnPortParam_Click(object sender, EventArgs e) {
            this.flowLayoutPanel6.VerticalScroll.Value = 0;
        }
        private void btnGoParam_Click(object sender, EventArgs e) {
            this.flowLayoutPanel6.VerticalScroll.Value = 130;
        }

        private void rioWay_Click(object sender, EventArgs e) {
            this.flowLayoutPanel6.VerticalScroll.Value = 320;
        }

        private void rioCar_Click(object sender, EventArgs e) {
            this.flowLayoutPanel6.VerticalScroll.Value = 320;
        }

        private void btnPortParam_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
