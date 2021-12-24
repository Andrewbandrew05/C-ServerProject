using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Client
{
    public class messageSerializer
    {
        public static byte[] serializeMessage(object messageToSerialize)
        {
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string JSonString=JsonSerializer.Serialize(messageToSerialize, options);
            System.Console.WriteLine(JSonString);
            return Encoding.ASCII.GetBytes(JSonString);                     
        }
        public static Message deserializeMessage(byte[] messageToDeserialize)
        {
            System.Console.WriteLine(Encoding.ASCII.GetString(messageToDeserialize));
            return JsonSerializer.Deserialize<Message>(Encoding.ASCII.GetString(messageToDeserialize));
        }
    }
}
