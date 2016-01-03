using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LittleNet.UsbMissile;

namespace WindowsFormsWP7
{
    static class StaticMissile
    {

        public static MissileDevice MissileLauncher;


        static StaticMissile()
        {
            MissileLauncher = new MissileDevice();
        }
    }
}
