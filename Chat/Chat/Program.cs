using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(new ThreadStart(Receive)).Start();
            Send();
        }


        static void Send()
        {
            TcpClient cl = new TcpClient();
            cl.Connect("localhost", 1666);
            while (true)
            {
                string x = Console.ReadLine();
                cl.GetStream().Write(Encoding.Unicode.GetBytes(x),0,x.Length * 2);
              
            }
            cl.Close();
        }


        static void Receive()
        {
            TcpListener li = new TcpListener(1666);
            li.Start();
            TcpClient cl = li.AcceptTcpClient();
            while (true)
            { 
                byte[] b = new byte[256];
                cl.GetStream().Read(b, 0, 256);
                Console.WriteLine(Encoding.Unicode.GetString(b));
            }
        
        }
    }
}
