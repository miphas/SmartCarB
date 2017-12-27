using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    public class SimPoint
    {
        public double x { get; set; }
        public double y { get; set; }

        /// <summary>
        /// Get the distance between two points
        /// </summary>
        public double getDis(SimPoint point)
        {
            return this.getDis(point.x, point.y);
        }
        /// <summary>
        /// Get the distance between two points
        /// </summary>
        public double getDis(double x, double y)
        {
            double dx = this.x - x;
            double dy = this.y - y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
