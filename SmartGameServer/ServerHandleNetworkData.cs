using Bindings;
using Newtonsoft.Json;
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
            Console.WriteLine("Initialize Network Packages...");
            Packets = new Dictionary<int, Packet_>
            {
                { (int)ClientPackets.cThankYou, HandleThankYou },
                { (int)ClientPackets.cData, HandleData },
                { (int)ClientPackets.cJoinRoom, JoinRoom },
                { (int)ClientPackets.cShareObject, ShareGameObjectData }
            };
        }

        public static void HandleNetworkInformation(int index, byte[] data)
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

        private static void HandleData(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            //Add your code you want to execute here
            Console.WriteLine(msg);
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

        private static void ShareGameObjectData(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            //{      "gameObject": { "id": "46546", "name": "N", "positionX": 12.2, "positionY": 12.2, "positionZ": 12.2, "rotationX": 12.2, "rotationY": 12.2, "rotationZ": 12.2 } }
            string msg = buffer.ReadString();
            buffer.Dispose();

            dynamic stuff = JsonConvert.DeserializeObject(msg);

            //Console.WriteLine("stuff.roomId " + stuff.gameObject.positionX);

            Vector3 position = new Vector3((float)stuff.gameObject.positionX, (float)stuff.gameObject.positionY, (float)stuff.gameObject.positionZ);
            Rotation rotation = new Rotation((float)stuff.gameObject.rotationX, (float)stuff.gameObject.rotationY, (float)stuff.gameObject.rotationZ);

            GameObject go = new GameObject() { Id = stuff.gameObject.id, Name = stuff.gameObject.name, Rotation = rotation, Vector = position };
            Console.WriteLine();
            ServerTCP.rooms[(int)stuff.roomId].ShareGameObject(ServerTCP.clients[index], go);

        }

        private static void JoinRoom(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            buffer.Dispose();


            int roomIndx = -1;
            //FindEmpetyRoom
            for (int i = 0; i < ServerTCP.rooms.Count; i++)
            {
                Room r = ServerTCP.rooms[i];
                for (int j = 0; j < r.clients.Length; j++)
                {
                    if (r.clients[j].socket == null)
                    {
                        roomIndx = i;
                        r.AddClientToRoom(ServerTCP.clients[index]);
                        //r.clients[j] = ServerTCP.clients[index];
                        break;
                    }
                }
            }
            if (roomIndx == -1)
            {
                Room room = new Room();
                room.roomID = ServerTCP.rooms.Count;
                room.AddClientToRoom(ServerTCP.clients[index]);
                ServerTCP.rooms.Add(room);
                roomIndx = ServerTCP.rooms.Count - 1;
            }
        }
    }
}
