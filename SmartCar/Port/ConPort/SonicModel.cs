using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar {
    public class SonicModel {
        // save copy value
        private int[] s;
        /// <summary>
        /// Sonic data
        /// </summary>
        public int[] S {
            get { return s; }
            set { s = value; }
        }
        /// <summary>
        /// L: left F: Front R: Right B: Back
        /// </summary>
        public enum SType{
            LFLeft, LFFront, RFFront, RFRight, RBRight, RBBack, LBBack, LBLeft
        }
        /// <summary>
        /// Construction of SonicModel
        /// </summary>
        /// <param name="data">The data that has been measured</param>
        public SonicModel(params int[] data) {
            // new array of measured value
            s = new int[data.Length];
            // copy the value
            for (int i = 0; i < s.Length; ++i) {
                s[i] = data[i];
            }
        }

    }
}
