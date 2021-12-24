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
    [SerializableAttribute]
    public class Message : ISerializable
    { 
        public string identifier { get; }
        public string usernameOfSender { get; }
        public string message { get; }
        public string usernameOfTarget { get; }
        public Message(string identifier, string message, string usernameOfTarget, string usernameOfSender)
        {
            this.identifier = identifier;
            this.usernameOfSender = usernameOfSender;
            this.message = message;
            this.usernameOfTarget = usernameOfTarget;
        }
        public Message(string identifier, string usernameOfSender)
        {
            this.identifier = identifier;
            this.usernameOfSender = usernameOfSender;
        }
        public Message(string identifier, string message, string usernameOfSender)
        {
            this.identifier = identifier;
            this.usernameOfSender = usernameOfSender;
            this.message = message;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("usernameOfSender", usernameOfSender);
            info.AddValue("identifier", identifier);
            info.AddValue("message", message);
            info.AddValue("usernameOfTarget", usernameOfTarget);
        }
        public Message(SerializationInfo info, StreamingContext context)
        {
            usernameOfSender = (string)info.GetValue("usernameOfSender", typeof(string));
            usernameOfTarget = (string)info.GetValue("usernameOfTarget", typeof(string));
            identifier = (string)info.GetValue("identifier", typeof(string));
            message = (string)info.GetValue("message", typeof(string));
        }
        public Message(){}
    }
    class Data
    {
        public static string username;
    }
}
