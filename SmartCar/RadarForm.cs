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
        public RadarForm()
        {
            InitializeComponent();

            //timerMethod += new timerDel()
        }

        private System.Timers.Timer timer = new System.Timers.Timer();
        private void button1_Click(object sender, EventArgs e)
        {
            PortManager.initPort();
            PortManager.urgPort.openPort();

            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            List<long> data = PortManager.urgPort.getUrgData().Distance;

            this.Invoke(new timerDel(refreshBoard), data);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PortManager.urgPort.closePort();

            timer.Close();
        }

        private delegate void timerDel(List<long> data);
        //private static timerDel timerMethod;

        private void refreshBoard(List<long> data)
        {
            this.label1.Text = "" + (data.Count > 384 ? data[384] : 0);
            this.pictureBox1.Image = new Draw.DrawRadar(400, 400).drawRadarImg(data);
        }

    }
}
