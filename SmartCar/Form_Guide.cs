using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SmartCar {
    public partial class Form_Guide : Form {
        public Form_Guide() {
            InitializeComponent();

            updatePathMethod += new updatePathDel(this.updatePath);
        }

        private bool isStart = false;
        private void btnStart_CheckedChanged(object sender, EventArgs e) {
            
        }

        /// <summary>
        /// 开始按钮事件
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e) {
            if (this.textBox2.Text.Length == 0)
            {
                MessageBox.Show("请打开路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!isStart) {
                // 改变为暂停样式
                this.btnStart.Image = SmartCar.Properties.Resources.pause_player_72px_15410_easyicon_net;
                this.btnStart.Text = "暂停";
                // 急停恢复
                if (PortManager.conPort != null) {
                    PortManager.conPort.Control_Continue();
                }
                // 
                ControlMethod.curState = ControlMethod.ctrlItem.GoMap;
            }
            else {
                // 改变为启动样式
                this.btnStart.Image = SmartCar.Properties.Resources.play_player_72px_15907_easyicon_net;
                this.btnStart.Text = "启动";
                // 急停恢复
                if (PortManager.conPort != null) {
                    PortManager.conPort.Control_Stop();
                }
            }
            isStart = !isStart;
        }

        /// <summary>
        /// 终止按钮点击
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e) {
            // 停止车运行
            if (PortManager.conPort != null) {
                PortManager.conPort.Control_Stop();
            }
        }

        /// <summary>
        /// 打开路径按钮
        /// </summary>
        private void btnPath_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                // 打开对应文件
                DataArea.mapFile = new MapFile(dialog.FileName);
                DataArea.mapModel = new MapModel();
                DataArea.mapFile.readNodeData(DataArea.mapModel);
                // 填写相关信息
                this.textBox1.Text = FilenameUtil.getFilename(dialog.FileName, ".xml");
                this.textBox2.Text = dialog.FileName;
                // 绘制路径
                DataArea.drawFormat.selfAdjustSize(DataArea.mapModel);
                DrawPath path = new DrawPath(DataArea.mapModel, DataArea.drawFormat);
                path.drawCurPos(new KeyPoint());
                this.picPath.Image = path.getDrawImg();
                this.picPath.BackgroundImage = new DrawPolygon(DataArea.mapModel, DataArea.drawFormat).getDrawImg();
            }
        }

        /// <summary>
        /// 更新路径委托
        /// </summary>
        /// <param name="curPos"></param>
        public delegate void updatePathDel(KeyPoint curPos);
        public static updatePathDel updatePathMethod;
        private void updatePath(KeyPoint curPos) {
            DrawPath path = new DrawPath(DataArea.mapModel, DataArea.drawFormat);
            path.drawCurPos(curPos);
            this.picPath.Image = path.getDrawImg();
        }







        private void showHint(int sel, int xloc, int yloc)
        {
            int npt = DataArea.mapModel.Points == null ? 0 : DataArea.mapModel.Points.Count;
            if (sel < 0 || sel >= npt) { this.panel1.Visible = false; return; }
            hintMsg.Initializing = true;
            
            KeyPoint pt = DataArea.mapModel.Points[sel];
            hintMsg.pCurrent = pt;
            this.comboBox1.Items.Clear();
            for (int i = 0; i < DataArea.mapModel.Points.Count; i++) { this.comboBox1.Items.Add(i.ToString()); }
            this.comboBox1.SelectedIndex = sel;
            this.textBox5.Text = pt.x.ToString();
            this.textBox4.Text = pt.y.ToString();
            this.textBox3.Text = pt.w.ToString();
            
            int x = this.picPath.Location.X + xloc;
            int y = this.picPath.Location.Y + yloc;
            this.panel1.Visible = true;
            this.panel1.Location = new Point(x, y);

            this.textBox3.ReadOnly = sel == 0;
            this.textBox4.ReadOnly = sel == 0;
            this.textBox5.ReadOnly = sel == 0;

            hintMsg.Initializing = false;
        }
        private HINT_MSG hintMsg;
        private struct HINT_MSG
        {
            /// <summary>
            /// 鼠标左键
            /// </summary>
            public bool Down1;
            /// <summary>
            /// 鼠标右键
            /// </summary>
            public bool Down2;
            /// <summary>
            /// 鼠标中间键
            /// </summary>
            public bool Down3;
            public bool Up1;
            public bool Up2;
            public bool Up3;

            public Point pDown1;
            public Point pDown2;
            public Point pDown3;
            public Point pUp1;
            public Point pUp2;
            public Point pUp3;

            public Point pHint;
            
            public KeyPoint pCurrent;

            public bool Initializing;
        }
        private void evtClickedPictureBox(object sender, EventArgs e)
        {
            Point ptMouse = this.picPath.PointToClient(MousePosition);

            DrawPath path = new DrawPath(DataArea.mapModel, DataArea.drawFormat);
            List<Point> ptKey = path.getPtLocInPic();
            int ptSize = DataArea.drawFormat.PointSize;if (ptSize < 3) { ptSize = 3; }

            int hint = -1;
            for (int i = 0; i < ptKey.Count; i++)
            {
                int xerr = Math.Abs(ptKey[i].X - ptMouse.X);
                int yerr = Math.Abs(ptKey[i].Y - ptMouse.Y);
                if (xerr <= ptSize && yerr <= ptSize)
                {
                    hint = i; break;
                }
            }

            showHint(hint, ptMouse.X, ptMouse.Y);
        }
        private void evtClickedBlankPanel(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
        }
        private void evtMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                hintMsg.Down1 = true;
                hintMsg.Up1 = false;
                hintMsg.pDown1 = MousePosition;
                hintMsg.pHint = this.panel1.Location;
            }
            if (e.Button == MouseButtons.Right)
            {
                hintMsg.Down2 = true;
                hintMsg.Up2 = false;
                hintMsg.pDown2 = MousePosition;
                hintMsg.pHint = this.panel1.Location;
            }
            if (e.Button == MouseButtons.Middle)
            {
                hintMsg.Down3 = true;
                hintMsg.Up3 = false;
                hintMsg.pDown3 = MousePosition;
                hintMsg.pHint = this.panel1.Location;
            }
        }
        private void evtMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                hintMsg.Down1 = false;
                hintMsg.Up1 = true;
                hintMsg.pUp1 = MousePosition;
                hintMsg.pHint = this.panel1.Location;
            }
        }
        private void evtMouseMove(object sender, MouseEventArgs e)
        {
            if (this.panel1.Visible && hintMsg.Down1)
            {
                int xMove = MousePosition.X - hintMsg.pDown1.X;
                int yMove = MousePosition.Y - hintMsg.pDown1.Y;
                this.panel1.Location = new Point(hintMsg.pHint.X + xMove, hintMsg.pHint.Y + yMove);
            }
        }
        private void evtPtChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == -1) { return; }
            if (hintMsg.Initializing) { return; }
            showHint(this.comboBox1.SelectedIndex, this.panel1.Location.X, this.panel1.Location.Y);
        }
        private void btnCancle(object sender, EventArgs e)
        {
            if (hintMsg.Initializing) { return; }
            this.panel1.Visible = false;
            //evtPtChanged(null, null);
        }
        private void btnOK(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == -1) { return; }
            if (this.comboBox1.SelectedIndex == 0) { return; }
            if (hintMsg.Initializing) { return; }

            try { hintMsg.pCurrent.x = double.Parse(this.textBox5.Text); } catch { }
            try { hintMsg.pCurrent.y = double.Parse(this.textBox4.Text); } catch { }
            try { hintMsg.pCurrent.w = double.Parse(this.textBox3.Text); } catch { }

            DataArea.mapModel.Points[this.comboBox1.SelectedIndex] = hintMsg.pCurrent;
            this.panel1.Visible = false;

            DataArea.drawFormat.selfAdjustSize(DataArea.mapModel);
            DrawPath path = new DrawPath(DataArea.mapModel, DataArea.drawFormat);
            path.drawCurPos(new KeyPoint());
            this.picPath.Image = path.getDrawImg();
            this.picPath.BackgroundImage = new DrawPolygon(DataArea.mapModel, DataArea.drawFormat).getDrawImg();
        }
        private void btnDelete(object sender, EventArgs e)
        {
            if (hintMsg.Initializing) { return; }
            if (this.comboBox1.SelectedIndex == 0) { return; }

            DataArea.mapModel.Points.RemoveAt(this.comboBox1.SelectedIndex);
            this.panel1.Visible = false;

            DataArea.drawFormat.selfAdjustSize(DataArea.mapModel);
            DrawPath path = new DrawPath(DataArea.mapModel, DataArea.drawFormat);
            path.drawCurPos(new KeyPoint());
            this.picPath.Image = path.getDrawImg();
            this.picPath.BackgroundImage = new DrawPolygon(DataArea.mapModel, DataArea.drawFormat).getDrawImg();
        }
    }
}
