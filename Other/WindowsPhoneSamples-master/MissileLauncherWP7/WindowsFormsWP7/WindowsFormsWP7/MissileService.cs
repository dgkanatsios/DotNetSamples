using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LittleNet.UsbMissile;
using System.Timers;

namespace WindowsFormsWP7
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MissileService" in both code and config file together.
    public class MissileService : IMissileService
    {

        public MissileService()
        {
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StaticMissile.MissileLauncher.Command(DeviceCommand.Stop);
        }



        public void Left()
        {

            StaticMissile.MissileLauncher.Command(DeviceCommand.Left);

            
        }

        

        public void Right()
        {
            
            StaticMissile.MissileLauncher.Command(DeviceCommand.Right);
    
            
        }

        public void Up()
        {

            StaticMissile.MissileLauncher.Command(DeviceCommand.Up);
 
        }

        public void Down()
        {

            StaticMissile.MissileLauncher.Command(DeviceCommand.Down);


            
        }


        public void Stop()
        {
            StaticMissile.MissileLauncher.Command(DeviceCommand.Stop);

        }

        public void Fire()
        {

            StaticMissile.MissileLauncher.Command(DeviceCommand.Fire);

            
        }
    }
}
