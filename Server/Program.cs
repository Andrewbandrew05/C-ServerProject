using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
namespace Server
{

    class Program
    {
        static void Main(string[] args)
        {
            int port = 50602;
            while (true)
            {

                try
                {
                    IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddr=null;
                    foreach (var ip in ipHost.AddressList)
                    {
                        if (ip.AddressFamily==AddressFamily.InterNetwork)
                        {
                            ipAddr = ip;
                        }
                    }
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);
                    Socket serverSocket = new Socket(ipAddr.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                    System.Console.WriteLine("Socket running on port " + port);
                    serverSocket.Bind(localEndPoint);
                    System.Console.WriteLine("Accepting Connections");
                    System.Console.WriteLine(ipAddr);
                    
                    serverSocket.Listen(10);
                    while (true)
                    {
                        Socket clientSocket = serverSocket.Accept();
                        Thread t = new Thread(() => Login.login(clientSocket));
                        t.Start();
                        System.Console.WriteLine("Client accepted.");
                    }
                }
                catch
                {
                    port += 1;
                    System.Console.WriteLine("Connect fail");
                }
            }
        }
    }
}
