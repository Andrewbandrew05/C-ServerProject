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
    class Communicator
    {
        private static Boolean new_message_about_users_from_server = false;
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
        private static void recieveMessages(Socket clientSocket)
        {
            while (true)
            {
                byte[] messageRecieved = new byte[1024];
                int byteRecv = clientSocket.Receive(messageRecieved);
                messages.Add((Message)messageSerializer.deserializeMessage(messageRecieved));
                if (((Message)messageSerializer.deserializeMessage(messageRecieved)).identifier == "user_list") 
                {
                    new_message_about_users_from_server = true;
                }
            }
        }
        public static void communicator(Socket clientSocket)
        {
            Thread t = new Thread(() => recieveMessages(clientSocket));
            t.Start();
            while (true)
            {
                Console.WriteLine("Would you like to view your messages or send a message?");
                string input = Console.ReadLine();
                if (input.ToLower().Contains("read") || input.ToLower().Contains("view"))
                {
                    int num_messages = 0;
                    string people_who_sent_messages = "";
                    foreach (Message message in messages)
                    {
                        if (message.usernameOfSender != "server")
                        {
                            num_messages += 1;
                            people_who_sent_messages = people_who_sent_messages + " " + message.usernameOfSender;
                        }
                    }
                    if (people_who_sent_messages.Length > 0)
                    {
                        Console.WriteLine("You have " + num_messages + " messages. They are from" + people_who_sent_messages + ". Whose message would you like to read?");
                        input = Console.ReadLine();
                        Boolean succesful_message_find = false;
                        foreach (Message message in messages.ToArray())
                        {
                            if (input.ToLower().Contains(message.usernameOfSender.ToLower()))
                            {
                                Console.WriteLine(message.message);
                                Console.WriteLine("Would you now like to remove this message from your inbox or keep it there?");
                                input = Console.ReadLine();
                                if (input.ToLower().Contains("remove"))
                                {
                                    messages.Remove(message);
                                }
                                succesful_message_find = true;
                            }
                        }
                        if (succesful_message_find == false)
                        {
                            Console.WriteLine("Sorry but we could not find your message.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You do not have any messages.");
                    }

                }
                else if (input.ToLower().Contains("send"))
                {

                    Console.WriteLine("Who would you like to send a message to?");
                    clientSocket.Send(messageSerializer.serializeMessage(new Message("user_request", null, "server", Data.username)));
                    string users = "";
                    while (true)
                    {
                        if (new_message_about_users_from_server == false) 
                        {
                            continue;
                            
                        }
                        Boolean succesful_find = false;
                        foreach (Message message in messages.ToArray())
                        {
                            if (message.identifier == "user_list")
                            {
                                Console.WriteLine("Here are the users you can message. " + message.message);
                                users = message.message;
                                succesful_find = true;
                            }
                        }
                        if (succesful_find == true)
                        {
                            break;
                        }
                    }

                    string username_of_recipient = Console.ReadLine();
                    if (users.Contains(username_of_recipient))
                    {
                        Console.WriteLine("What message would you like to send?");
                        string message_to_send = Console.ReadLine();
                        clientSocket.Send(messageSerializer.serializeMessage(new Message("default_message", message_to_send, username_of_recipient, Data.username)));

                    }
                    else
                    {
                        Console.WriteLine("Sorry but we could not find that username.");
                    }
                }
        }
    }
}
}
