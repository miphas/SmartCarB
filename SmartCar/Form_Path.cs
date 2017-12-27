using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartCar {
    public partial class Form_Path : Form {

        public delegate void refreshMapDel(Image map, Image back);
        public static refreshMapDel refreshMethod;
        private ProcessNewMap proNewMap;

        // 通道居中 靠左 靠右行驶
        public static int wayType = 0;

        // 出通道以后是否返回
        public static bool wayBack = false;
        public Form_Path() {
            InitializeComponent();

            refreshMethod = new refreshMapDel(this.updateMap);
        }

        /// <summary>
        /// 选择模式旋钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void boxModel_SelectedIndexChanged(object sender, EventArgs e) {
            // 选择生成地图选项
            if (boxModel.SelectedIndex == 0) {
                // 初始化串口信息
                PortManager.initPort();
                PortManager.openAllPort();
                ProcessNewMap.init();
                ProcessNewMap.startRefreshMap();
            }
        }

        private void updateMap(Image img, Image back) {
            this.picGenMap.Image = img;
            this.picGenMap.BackgroundImage = back;
        }

        /// <summary>
        /// 记录普通关键点
        /// </summary>
        private void rioPT_Click(object sender, EventArgs e) {
            if (this.boxModel.Text.Length == 0) {
                MessageBox.Show("请先选择模式!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ProcessNewMap.markKeyPoint(0);
        }

        /// <summary>
        /// 记录雷达关键点
        /// </summary>
        private void rioLD_Click(object sender, EventArgs e) {
            //ProcessNewMap.markKeyPoint(1);
            if (this.boxModel.Text.Length == 0) {
                MessageBox.Show("请先选择模式!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ControlMethod.curState = ControlMethod.ctrlItem.ExpMap;
        }

        /// <summary>
        /// 保存当前路径
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e) {
            if (this.boxModel.SelectedIndex == -1) {
                MessageBox.Show("请选择模式！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.txtName.Text.Length == 0) {
                MessageBox.Show("请填写路径名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 新建路径模式下
            if (this.boxModel.SelectedIndex == 0) {
                FolderBrowserDialog dia = new FolderBrowserDialog();
                dia.ShowDialog();
                if (dia.SelectedPath == null || dia.SelectedPath.Length == 0) {
                    MessageBox.Show("请选择路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ProcessNewMap.stopRefreshMap();
                String path = dia.SelectedPath + (dia.SelectedPath.EndsWith("\\") ? "" : "\\") + txtName.Text + ".xml";
                MapFile mf = new MapFile(path);
                MapModel tm = ProcessNewMap.getMapModel();
                mf.writeNodeData(tm);

            }
        }

        /// <summary>
        /// 切换行进模式
        /// </summary>
        private void boxWay_SelectedIndexChanged(object sender, EventArgs e) {
            Form_Path.wayType = this.boxWay.SelectedIndex;
        }

        private void rioDLJ_Click(object sender, EventArgs e) {
            if (this.boxModel.Text.Length == 0) {
                MessageBox.Show("请先选择模式!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Form_Path.wayBack = false;
            ControlMethod.curState = ControlMethod.ctrlItem.ExpMap;
        }

        private void rioSLJ_Click(object sender, EventArgs e) {
            if (this.boxModel.Text.Length == 0) {
                MessageBox.Show("请先选择模式!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Form_Path.wayBack = true;
            ControlMethod.curState = ControlMethod.ctrlItem.ExpMap;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 打开对应文件
                DataArea.mapFile = new MapFile(dialog.FileName);
                DataArea.mapModel = new MapModel();
                DataArea.mapFile.readNodeData(DataArea.mapModel);
                // 填写相关信息
                //this.textBox1.Text = FilenameUtil.getFilename(dialog.FileName, ".xml");
                //this.textBox2.Text = dialog.FileName;
                // 绘制路径
                DataArea.drawFormat.selfAdjustSize(DataArea.mapModel);
                DrawPath path = new DrawPath(DataArea.mapModel, DataArea.drawFormat);
                path.drawCurPos(new KeyPoint());
                this.picGenMap.Image = path.getDrawImg();
                this.picGenMap.BackgroundImage = new DrawPolygon(DataArea.mapModel, DataArea.drawFormat).getDrawImg();
            }
        }




        

    }
}
