using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace Server
{
    public class Login
    {
        private static List<User> loginQue = new List<User>();
        public static void createUserAccount(Socket clientSocket)
        {
            clientSocket.Send(messageSerializer.serializeMessage(new Message("username_request", null, null, "server")));
            byte[] messageRecieved = new byte[1024];
            int byteRecv = clientSocket.Receive(messageRecieved);
            Message usernameMessage = (Message)messageSerializer.deserializeMessage(messageRecieved);
            string username_of_new_user = usernameMessage.message;
            clientSocket.Send(messageSerializer.serializeMessage(new Message("password_request", null, null, "server")));
            messageRecieved = new byte[1024];
            byteRecv = clientSocket.Receive(messageRecieved);
            Message passwordMessage = (Message)messageSerializer.deserializeMessage(messageRecieved);
            string password_of_new_user = passwordMessage.message;
            Data.Users.Add(username_of_new_user, new User(username_of_new_user, password_of_new_user));
        }
        public static void loginUser(Socket clientSocket, Message usernameMessage)
        {
            
            foreach (KeyValuePair<string, User> kvp in Server.Data.Users)
            {
                if (usernameMessage.message == kvp.Key)
                {
                    clientSocket.Send(messageSerializer.serializeMessage(new Message("password_request", null, usernameMessage.usernameOfSender, "server")));
                    loginQue.Add(Data.Users[usernameMessage.usernameOfSender]);
                }
            }
            byte[] messageRecieved = new byte[1024];
            int byteRecv = clientSocket.Receive(messageRecieved);
            Message passwordMessage = (Message)messageSerializer.deserializeMessage(messageRecieved);
            foreach (User userLoggingIn in loginQue)
            {
                if (passwordMessage.message == userLoggingIn.password)
                {
                    onlineUser loggedInUser = new onlineUser(userLoggingIn.username, userLoggingIn.password, clientSocket);
                    Data.onlineUsers.Add(loggedInUser.username, loggedInUser);
                    ClientCommunicator.clientCommunicator(loggedInUser);
                }
            }
        }
        public static void login(Socket clientSocket)
        {
            System.Console.Write("chgeck");
            clientSocket.Send(messageSerializer.serializeMessage(new Message("username_request","server")));
            System.Console.Write("message sent");
            byte[] messageRecieved = new byte[1024];
            int byteRecv = clientSocket.Receive(messageRecieved);
            Message usernameMessage = (Message)messageSerializer.deserializeMessage(messageRecieved);
            if (usernameMessage.identifier == "username")
            {
                loginUser(clientSocket, usernameMessage);
            }
            else if(usernameMessage.identifier=="create_account_request")
            {
                createUserAccount(clientSocket);
            }
        }
    }
}
