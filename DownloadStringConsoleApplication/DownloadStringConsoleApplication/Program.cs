using System;
using System.Net;
using System.Text.RegularExpressions;

namespace DownloadStringConsoleApplication
{
    class Program
    {
        static void Main()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri("http://google.com"));

            Console.ReadLine();
        }

        static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                return;
            }

            string data = e.Result;
            Regex regEx = new Regex("<[^>]*>");

            string[] cleanedData = regEx.Split(data);

            foreach (string cleanData in cleanedData)
            {
                Console.WriteLine(cleanData);
            }
        }
    }
}
