using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters;
namespace Client
{
    class Login
    {
        public void Begin_Login(Socket clientSocket)
        {
            byte[] messageRecieved = new byte[1024];
            clientSocket.Receive(messageRecieved);
            Message loginRequest = (Message)messageSerializer.deserializeMessage(messageRecieved);
            System.Console.Write(loginRequest.identifier);
            if (loginRequest.identifier == "username_request")
            {
                Console.WriteLine("Please type your username. If you do not have one, please type 'create account'.");
                string username = Console.ReadLine();
                if (username == "create account")
                {
                    create_Account(clientSocket);
                }
                else 
                {
                    commence_Login(clientSocket, username);
                }
            }
        }
        public void create_Account(Socket clientSocket) 
        {
            clientSocket.Send(messageSerializer.serializeMessage(new Message("create_account_request", null, "server", null)));
            byte[] messageRecieved = new byte[1024];
            int byteRecv = clientSocket.Receive(messageRecieved);
            Message username_Request = (Message)messageSerializer.deserializeMessage(messageRecieved);
            if (username_Request.identifier == "username_request") 
            {
                while (true)
                {
                    Console.WriteLine("Please type your username now. It can not be 'create account', 'server', or a username already in use.");
                    string username = Console.ReadLine();
                    if (username == "server" || username == "create account")
                    {
                        Console.WriteLine("Your username may not be 'create account' or 'server'.");
                    }
                    else 
                    {
                        clientSocket.Send(messageSerializer.serializeMessage(new Message("username", username, "server", null)));
                        messageRecieved = new byte[1024];
                        byteRecv = clientSocket.Receive(messageRecieved);
                        Message password_Request = (Message)messageSerializer.deserializeMessage(messageRecieved);
                        if (password_Request.identifier == "password_request") 
                        {
                            Console.WriteLine("Please type in your password.");
                            string password=Console.ReadLine();
                            clientSocket.Send(messageSerializer.serializeMessage(new Message("password", password, "server", null)));
                            Console.WriteLine("Your username is " + username + " and your password is " + password + ". Please write this information down somewhere, because if you forget your information you will not be able to reset your password or username until further notice.");
                            Begin_Login(clientSocket);
                        }
                        else 
                        {
                            Console.WriteLine("Sorry, but that username is already in use, please try another one.");
                        }
                    }
                }
            }
        }
        public void commence_Login(Socket clientSocket, String username) 
        {
            while (true)
            {
                clientSocket.Send(messageSerializer.serializeMessage(new Message("username", username, "server", username)));
                byte[] messageRecieved = new byte[1024];
                int byteRecv = clientSocket.Receive(messageRecieved);
                Message passwordRequest = (Message)messageSerializer.deserializeMessage(messageRecieved);
                if (passwordRequest.identifier == "password_request")
                {
                    Data.username = username;
                    Console.WriteLine("Please type in your password.");
                    while (true)
                    {
                        string password = Console.ReadLine();
                        clientSocket.Send(messageSerializer.serializeMessage(new Message("password", password, "server", username)));
                        messageRecieved = new byte[1024];
                        byteRecv = clientSocket.Receive(messageRecieved);
                        Message login_Check = (Message)messageSerializer.deserializeMessage(messageRecieved);
                        if (login_Check.identifier == "login_succesful")
                        {
                            Console.WriteLine("Login succesful.");
                            Communicator.communicator(clientSocket);
                            break;
                        }
                        else if (login_Check.identifier == "password_request")
                        {
                            Console.WriteLine("You typed your password incorrectly. Please try again.");
                        }
                    }
                    break;
                }
                else if (passwordRequest.identifier == "username_request")
                {
                    Console.WriteLine("You typed your username incorrectly. Please try again.");
                    username = Console.ReadLine();
                }
            }
        }
    }
}
