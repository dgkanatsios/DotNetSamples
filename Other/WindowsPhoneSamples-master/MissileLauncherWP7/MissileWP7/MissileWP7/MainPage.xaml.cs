using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MissileWP7.MissileServiceReference;
using System.ServiceModel;
using System.Diagnostics;

namespace MissileWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            client = new MissileServiceClient(new BasicHttpBinding(), new EndpointAddress("http://v-digkan2:31337/MissileService"));
        }

        MissileServiceClient client;

        private void leftBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            

            client.LeftAsync();

        }

        private void rightBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            client.RightAsync();
        }

        private void downBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            

            client.DownAsync();
        }

        private void fireBtn_Click(object sender, RoutedEventArgs e)
        {
            

            client.FireAsync();
        }

        private void upBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            

            client.UpAsync();
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            
            client.StopAsync();
        }

        private void mouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("in main page, mouse up");
            client.StopAsync();
        }

       

        
    }
}