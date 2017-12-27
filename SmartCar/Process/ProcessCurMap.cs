using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class ProcessCurMap {
        public static void mapping() {
            if (DataArea.mapModel == null || PortManager.drPort == null) {
                return;
            }
            Form1.MForms[0].Invoke(Form_Guide.updatePathMethod, PortManager.drPort.getPosition());
        }
    }
}
