using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmartCar {
    public abstract class DrawM {
        public abstract void draw(MapModel map);

        public abstract Image getDrawImg();

    }
}
