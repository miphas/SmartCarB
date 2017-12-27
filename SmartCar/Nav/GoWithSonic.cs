using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{ 
     class GoWithSonic
    {
        
        //ConPort con_port = ConPort.getInstance();
        SonicModel sonicmodel = new SonicModel();
         public void goWithSonic(KeyPoint res, KeyPoint des, IConPort myConPort, IDrPort myDrPort)
        {
           
        
            int goSpeed = -100;
            double toMove = 0;
            bool flagX = false;
            bool flagY = false;
            int shiftSpeed = 0;
            int rotatSpeed = 0;
            KeyPoint start = myDrPort.getPosition();
            KeyPoint now = myDrPort.getPosition();
            
            if (Math.Abs(des.x - res.x) < Math.Abs(des.y - res.y))
            {
                toMove = des.y - res.y;
                flagY = true;
            }
            else
            {
                toMove = des.x - res.x;
                flagX = true;
            }
            while ((flagX && Math.Abs(toMove - (now.x - start.x)) > 0.1) ||
                   (flagY && Math.Abs(toMove - (now.y - start.y)) > 0.1))
            {

                GetBackInfo(ref shiftSpeed, ref rotatSpeed,myConPort);
                //if (myUrgPort.CanGo())
                //{
                    myConPort.Control_Move_By_Speed(goSpeed, -shiftSpeed, rotatSpeed/60);
                //}
                System.Threading.Thread.Sleep(100);
                now = myDrPort.getPosition();
               
            }

        }

        private void GetBackInfo(ref int shiftSpeed, ref int rotatSpeed,IConPort myconport)
        {
           SonicModel sm = myconport.Measure_Sonic();
            int right = sm.S[4];
            int left = sm.S[7];
            //if (right < 80) right = 230;
            //if (left < 80) left = 230;
            double angle;
            double dis;
            double length = right + left + 400;//左右距离+车宽
            if (right > 350 && left > 350)
            {
                rotatSpeed = 0;
                shiftSpeed = 0;
            }
            else if (right > 350)
            {
                shiftSpeed = (int)(0.2 * (200 - left));
                rotatSpeed = (int)(3 * (200 - left));
            }
            else if (left > 350)
            {
                shiftSpeed = (int)(0.2 * (right - 200));
                rotatSpeed = (int)(3 * (right - 200));

            }
            else
            {
                if (length < 800)
                    angle = 0;
                else angle = Math.Acos(800 / length);
                if (right >= left)
                {
                    rotatSpeed = (int)(5 * (angle * 180 / 3.14159));

                }
                else if (right < left)
                {
                    rotatSpeed = -(int)(5 * (angle * 180 / 3.14159));
                }
                if (length > 800)
                    shiftSpeed = (int)(0.2 * (right - left));
            }
        }
    }
}
