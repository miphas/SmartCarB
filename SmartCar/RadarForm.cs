using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;

namespace SmartCar
{
    public partial class RadarForm : Form
    {
        public static bool record = false;
        public RadarForm()
        {
            InitializeComponent();

        }

        private System.Timers.Timer timer = new System.Timers.Timer();
        private void button1_Click(object sender, EventArgs e)
        {
            PortManager.initPort();
            PortManager.urgPort.openPort();
            PortManager.drPort.openPort();
            PortManager.drPort.setPosition(0, 0, 0);

            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            cnt = 0;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            record = true;
            //throw new NotImplementedException();
            List<long> data = PortManager.urgPort.getUrgData().Distance;

            this.Invoke(new timerDel(refreshBoard), data);

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            record = false;
            
            PortManager.urgPort.closePort();
            PortManager.drPort.closePort();

            timer.Close();
            Util.ExpData.closeFile();
        }

        private delegate void timerDel(List<long> data);
        //private static timerDel timerMethod;

        private static int cnt = 0;
        private void refreshBoard(List<long> data)
        {
            this.label1.Text = "" + (data.Count > 340 ? data[340] : 0);
            //this.label5.Text = "" + (data.Count > 383 ? data[383] : 0);
            this.pictureBox1.Image = new Draw.DrawRadar(400, 400).drawRadarImg(data);

            cnt++;
            this.label2.Text = cnt.ToString();

            //this.label3.Text = "" + (data.Count > 310 ? data[310] : 0);
            //this.label4.Text = "" + (data.Count > 370 ? data[370] : 0);
            this.label3.Text = "" + (data.Count > 370 ? (data[370] - data[310]) : 0);

            var pos = PortManager.drPort.getPosition();
            this.label6.Text = pos.x.ToString("F3");
            this.label7.Text = pos.y.ToString("F3");
            this.label8.Text = pos.w.ToString("F3");

        }

    }
}
