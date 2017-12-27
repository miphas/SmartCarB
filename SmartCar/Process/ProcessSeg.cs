using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmartCar
{
    public class ProcessSeg
    {
        // segment cut range value
        public static double range = 0.01; // 0.05

        // save current segment's left side and right side
        private List<SimPoint> leftPs = new List<SimPoint>();
        private List<SimPoint> righPs = new List<SimPoint>();

        private int leftEnd = -1;
        private int righEnd = -1;
        // add new left and rigth side point
        public void addPoint(SimPoint left, SimPoint righ)
        {
            int li = leftPs.Count - 1;
            int ri = righPs.Count - 1;
            if (li < 0 || ri < 0)
            {
                leftPs.Add(left);
                righPs.Add(righ);
            }
            else
            {
                if (leftPs[li].getDis(left) < 0.01 ||
                    righPs[ri].getDis(righ) < 0.01)
                {
                    return;
                }
                // 没有转向
                //if (MoveInfo.curTurn == MoveInfo.Turning.NoTurn)
                //{
                    leftPs.Add(left);
                    righPs.Add(righ);
                    //leftEnd = righEnd = -1;
                //}
                //// 当前转向为向左转
                //else if (MoveInfo.curTurn == MoveInfo.Turning.TrunLeft)
                //{
                //    leftEnd = (leftEnd == -1) ? leftPs.Count : leftEnd;
                //    leftPs.Insert(leftEnd, left);
                //    righPs.Add(righ);
                //}
                //else if (MoveInfo.curTurn == MoveInfo.Turning.TurnRight)
                //{
                //    righEnd = (righEnd == -1) ? righPs.Count : righEnd;
                //    righPs.Insert(righEnd, righ);
                //    leftPs.Add(left);
                //}
            }
        }

        // save the current segment
        public Segment getSegment()
        {
            // deal with real point
            Segment seg = new Segment();
            seg.LeftP = cutPoint(leftPs);
            seg.RightP = cutPoint(righPs);
            //leftPs = seg.LeftP;
            //righPs = seg.RightP;
            return seg;
        }

        /// <summary>
        /// 
        /// </summary>
        private void cutWorker(int start, int end, List<SimPoint> list, List<int> keys)
        {
            // too less points return
            if (end - start < 3)
            {
                return;
            }
            // recode the max value
            int maxI = start;
            double maxV = 0;
            // every step change in x and y
            double perX = (list[end].x - list[start].x) / (end - start);
            double perY = (list[end].y - list[start].y) / (end - start);
            // find the max distance
            for (int i = start + 1; i < end; ++i)
            {
                SimPoint p = list[i];
                double curD = p.getDis(list[start].x + perX * (i - start),
                                       list[start].y + perY * (i - start));
                // compare and get the distance
                if (curD > maxV)
                {
                    maxV = curD;
                    maxI = i;
                }
            }
            if (maxV < range)
            {
                return;
            }
            // recode the special point and check the next one
            keys.Add(maxI);
            cutWorker(start, maxI, list, keys);
            cutWorker(maxI, end, list, keys);
        }
        /// <summary>
        /// cut the useless point
        /// </summary>
        private List<SimPoint> cutPoint(List<SimPoint> list)
        {
            // return list with 0 item
            if (list.Count == 0)
            {
                return new List<SimPoint>();
            }
            // get cut point's information
            List<int> keys = new List<int>() { 0, list.Count - 1 };
            cutWorker(0, list.Count - 1, list, keys);
            keys.Sort();
            // collect the cut point information about key points
            List<SimPoint> ans = new List<SimPoint>();
            foreach (int k in keys)
            {
                ans.Add(list[k]);
            }
            return ans;
        }


    }
}
