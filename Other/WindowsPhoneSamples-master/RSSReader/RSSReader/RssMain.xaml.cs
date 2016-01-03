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
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSReader
{
    public partial class RssMain : PhoneApplicationPage
    {
        public RssMain()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(RssMain_Loaded);
        }

        void RssMain_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
            wc.OpenReadAsync(new Uri("http://www.studentguru.gr/blogs/dt008/rss.aspx"));
        }
               
        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            SyndicationFeed feed;
            using (XmlReader reader = XmlReader.Create(e.Result))
            {
                feed = SyndicationFeed.Load(reader);
                lst.ItemsSource = feed.Items;
            }
        }

        
    

    }
}
