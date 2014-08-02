using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace FileSystemWatcherTest
{
    class Program
    {

        static bool changeOccurred = false;

        static void Main(string[] args)
        {
            FileSystemWatcher fw = new FileSystemWatcher();
            fw.Path = @"C:\";
            fw.Filter = "*.txt";
            fw.Created += new FileSystemEventHandler(fw_Created);
            fw.EnableRaisingEvents = true;
            while (!changeOccurred)
            {
                Thread.Sleep(2000);
                Console.WriteLine("Waiting...");
            }
            Console.ReadLine();
            
        }

        static void fw_Created(object sender, FileSystemEventArgs e)
        {
            changeOccurred = true;
            Console.WriteLine("Change type: {0} && Path: {1}", e.ChangeType, e.FullPath); 
        }
    }
}
