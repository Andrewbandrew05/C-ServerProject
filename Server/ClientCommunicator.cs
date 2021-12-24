using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace Server
{
    
    class ClientCommunicator
    {
        private static List<Message> messages = new List<Message>();
        
        private static string decodeMessage(byte[] messageRecieved, int byteRecv)
        {
            string messageFromUser;
            try
            {
                messageFromUser = Encoding.ASCII.GetString(messageRecieved);
            }
            catch
            {
                try
                {
                    messageFromUser = Encoding.UTF8.GetString(messageRecieved);
                }
                catch
                {
                    messageFromUser = ("Message decoding fail.");
                }

            }
            return messageFromUser;
        }
        private static void recieveMessages(onlineUser User) 
        {
            while (true)
            {
                byte[] messageRecieved = new byte[1024];
                int byteRecv = User.clientSocket.Receive(messageRecieved);
                //string response = decodeMessage(messageRecieved, byteRecv); Implement this code later once encoding of serilaized object is figurd out.
                messages.Add((Message)messageSerializer.deserializeMessage(messageRecieved));
            }
        }
        private static void forwardMessages(onlineUser User) 
        {
            while (true) 
            {
                foreach (Message i in messages)
                {
                    if (i.identifier == "default_message")
                    {
                        Data.onlineUsers[i.usernameOfTarget].clientSocket.Send(messageSerializer.serializeMessage(i));
                    }
                }
            }
        }
        public static void clientCommunicator(onlineUser User) 
        {
            recieveMessages(User);
            forwardMessages(User);
        }
        

    }
}
