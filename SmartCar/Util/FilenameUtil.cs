using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class FilenameUtil {
        public static String getFilename(String pathAndName, String postfix) {
            int startPos = pathAndName.LastIndexOf("\\") + 1;
            int endPos = pathAndName.LastIndexOf(postfix);
            return pathAndName.Substring(startPos, endPos - startPos);
        }
    }
}
