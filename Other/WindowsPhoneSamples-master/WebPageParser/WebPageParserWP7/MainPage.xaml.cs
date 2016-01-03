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
using System.Threading;
using System.IO;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using HtmlAgilityPack;

namespace WebPageParserWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }


        Updates updates = new Updates();

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("el-GR");

            HtmlWeb.LoadAsync("http://www.minfin.gr/portal/", HtmlWeb_Loaded);
        }



        private void HtmlWeb_Loaded(object sender, HtmlDocumentLoadCompleted e)
        {
            if (e.Error == null)
            {
                HtmlDocument doc = e.Document;
                //get the latest updates
                var updatesHtmlList = (from y in doc.DocumentNode.Descendants("ul")
                            where y.Attributes.Contains("id") &&
                            y.Attributes["id"].Value == "latestUpdatesList"
                            select y).Single();

                //get all the strings from latest updates
                foreach (HtmlNode hn in updatesHtmlList.ChildNodes)
                {
                    Update up = new Update();
                    up.Datetime = DateTime.Parse(hn.InnerText.Split('-')[0]);
                    up.Title = hn.Element("a").InnerText.Substring(2);
                    up.HyperLink = "http://www.minfin.gr" + hn.Element("a").Attributes["href"].Value;
                    updates.Add(up);
                }

                updatesLst.ItemsSource = updates;
                
            }

        }


    }


    public class Update
    {
        public string Title { get; set; }
        public DateTime Datetime { get; set; }
        public string HyperLink { get; set; }



    }

    public class Updates : ObservableCollection<Update>
    { }
}