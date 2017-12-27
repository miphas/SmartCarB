using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    public class UrgInfo : Info
    {
        public static int startIndex = 44;

        public static int ang000Index = 85; // 129 - 44
        public static int ang180Index = 596; // 640 - 44

        public static int maxDist = 4000;

        public override void updateInfo() {
            // update nothing
        }

    }
}
