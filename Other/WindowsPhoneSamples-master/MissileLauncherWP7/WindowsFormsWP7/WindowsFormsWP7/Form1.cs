using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WindowsFormsWP7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StaticMissile.MissileLauncher.Dispose();
        }

        private ServiceHost HostProxy;



        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string address = "http://localhost:31337/MissileService";

                HostProxy = new ServiceHost(typeof(MissileService), new Uri(address));



                // Enable metadata publishing.

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();

                smb.HttpGetEnabled = true;

                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

                HostProxy.Description.Behaviors.Add(smb);



                // Open the ServiceHost to start listening for messages. Since

                // no endpoints are explicitly configured, the runtime will create

                // one endpoint per base address for each service contract implemented

                // by the service.



                HostProxy.Open();

                label1.Text = "The service is ready at " + address;

            }

            catch (AddressAccessDeniedException)
            {

                label1.Text = "You need to reserve the address for this service";

                HostProxy = null;

            }

            catch (AddressAlreadyInUseException)
            {

                label1.Text = "Something else is already using this address";

                HostProxy = null;

            }

            catch (Exception ex)
            {

                label1.Text = "Something bad happened on startup: " + ex.Message;

                HostProxy = null;

            }

        }


    }
}
