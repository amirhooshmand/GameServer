using Bindings;
using System;
using System.Collections.Generic;
namespace SmartGameServer
{
    class ServerHandleNetworkData
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initiakize Network Packages...");
            Packets = new Dictionary<int, Packet_>
            {
                { (int)ClientPackets.cThankYou, HandleThankYou }
            };
        }

        public static void HandleNetworkInformation(int index,  byte[] data)
        {
            int packetNum;
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            packetNum = buffer.ReadInteger();
            buffer.Dispose();
            if (Packets.TryGetValue(packetNum, out Packet_ Packet))
            {
                Packet.Invoke(index, data);
            }
        }

        private static void HandleThankYou(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            //Add your code you want to execute here
            Console.WriteLine(msg);
        }
    }
}
