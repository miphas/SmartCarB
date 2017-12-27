using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class SearchUtil {
        /// <summary>
        /// 寻找对应索引值
        /// </summary>
        /// <param name="club"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int getItemIndex(String[] club, String item) {
            for (int i = 0; i < club.Length; ++i) {
                if (item.Equals(club[i])) {
                    return i;
                }
            }
            return -1;
        }

    }
}
