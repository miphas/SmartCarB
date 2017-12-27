using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCar
{
    class Backward
    {
        private static List<COMMAND> Commands;
        public struct COMMAND { public int ForwardSpeed, LeftSpeed, RotateSpeed; }

        //public KeyPoint startpoint;

        public void clear()
        {
            if (Commands == null) { Commands = new List<COMMAND>(); return; }
            Commands.Clear();
        }
        public void set(COMMAND command)
        {
            if (Commands == null) { Commands = new List<COMMAND>(); }

            Commands.Add(command);
        }
        public COMMAND get()
        {
            if (Commands == null || Commands.Count == 0) { return new COMMAND(); }

            COMMAND command = Commands[Commands.Count - 1];
            Commands.RemoveAt(Commands.Count - 1);
            return command;
        }

        public void Start()
        {
            if (Commands == null) { return; }

            while (Commands.Count != 0)
            {
                COMMAND command = get();

                int wSpeed = command.RotateSpeed;

                // 判断是否到达
                KeyPoint currentpoint = PortManager.drPort.getPosition();
                //if (Math.Abs(currentpoint.x - startpoint.x) < 0.05) { break; }

                // 如果停车，则继续发送一次指令
                InfoManager.carIF.LastPoint = PortManager.drPort.getPosition();
                for (KeyPoint curPoint = new KeyPoint(InfoManager.carIF.LastPoint);
                     InfoManager.carIF.LastPoint.comparePos(curPoint);
                     curPoint = PortManager.drPort.getPosition())
                {
                    PortManager.conPort.Control_Move_By_Speed(-command.ForwardSpeed, -command.LeftSpeed, -wSpeed);
                    System.Threading.Thread.Sleep(100);

                    if(command.ForwardSpeed < 20 && command.LeftSpeed < 20)
                    {
                        break;
                    }
                }
                
            }

            PortManager.conPort.Control_Move_By_Speed(0, 0, 0);
            System.Threading.Thread.Sleep(1000);

        }
    }
}
