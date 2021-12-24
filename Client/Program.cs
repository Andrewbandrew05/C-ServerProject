using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Threading;
namespace Client
{
    class Program
    {
        private static Socket clientSocket;
        static void Main(string[] args)
        {
            Console.WriteLine("Please type the IPAdress of the machine you are attempting to connect to.");
            string input = Console.ReadLine();
            Console.WriteLine("Please type the port you would like to begin your search on.");
            int port = Convert.ToInt32(Console.ReadLine());
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Boolean connectionWasSuccesful = false;
            while (true)
            {
                if (port > 65535)
                {
                    break;
                }
                try
                {
                    clientSocket.Connect(IPAddress.Parse(input), port);
                    connectionWasSuccesful = true;
                    break;
                }
                catch
                {
                    port += 1;
                    Console.WriteLine(port);
                }
            }
            if (connectionWasSuccesful == true)
            {
                Console.WriteLine("You are connected to the server.");
                Login login = new Login();
                login.Begin_Login(clientSocket);
            }
            else
            {
                Console.WriteLine("We were not able to connect to the server.");
            }
        }
    }
}
