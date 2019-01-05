using Bindings;
using System;
using System.Collections.Generic;

namespace SmartClient
{
    class ClientHandleNetworkData
    {
        private delegate void Packet_(byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initialize Network Packages...");
            Packets = new Dictionary<int, Packet_>
            {
                { (int)ServerPackets.sConnectionOK, HandleConnectionOK },
                { (int)ServerPackets.sData, HandleData }
            };
        }

        public static void HandleNetworkInformation(byte[] data)
        {
            int packetNum;
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            packetNum = buffer.ReadInteger();
            buffer.Dispose();
            if (Packets.TryGetValue(packetNum, out Packet_ Packet))
            {
                Packet.Invoke(data);
            }
        }

        private static void HandleConnectionOK(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            //Add your code you want to execute here
            DateTime date = DateTime.Now;
            Console.WriteLine(date.TimeOfDay + " | " + msg);

            ClientTCP.JoinToRoom();
            //ClientTCP.SendGameobject();
        }
        private static void HandleData(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            //Add your code you want to execute here
            DateTime date = DateTime.Now;
            Console.WriteLine(date.TimeOfDay + " | " + msg);

            //ClientTCP.ThankYouServer();
        }

    }
}
