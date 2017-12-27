using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmartCar
{
    public class Segment
    {
        // Left side points and right side points
        private List<SimPoint> leftP = new List<SimPoint>();
        private List<SimPoint> rightP = new List<SimPoint>();

        // getter and setter for left and right side points
        public List<SimPoint> LeftP
        {
            get { return leftP; }
            set { leftP = value; }
        }
        public List<SimPoint> RightP
        {
            get { return rightP; }
            set { rightP = value; }
        }
    }
}
