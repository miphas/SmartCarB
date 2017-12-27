using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class MapModel {
        /// <summary>
        /// 点序列\ 可行区间
        /// </summary>
        private List<KeyPoint> points = new List<KeyPoint>();
        private List<Segment> segments = new List<Segment>();
        public List<KeyPoint> Points {
            get { return this.points; }
            set { this.points = value; }
        }
        public List<Segment> Segments
        {
            get { return this.segments; }
            set { this.segments = value; }
        }

        public MapModel() { }
        public MapModel(MapModel map) {
            this.Points = new List<KeyPoint>(map.Points);
            this.Segments = new List<Segment>(map.Segments);
        }

    }
}
