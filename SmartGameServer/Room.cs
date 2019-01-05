using Bindings;
using System;
using System.Collections.Generic;

namespace SmartGameServer
{ 
    class Room
    {
        public Client[] clients = new Client[Constants.ROOME_PAYER];
        public int roomID;
        public Room()
        {
            for (int i = 0; i < Constants.ROOME_PAYER; i++)
                clients[i] = new Client();
        }

        public void AddClientToRoom(Client client)
        {
            for (int i = 0; i < clients.Length; i++)
                if (clients[i].socket == null)
                {
                    clients[i] = client;
                    break;
                }

            Console.WriteLine("Player Connect to Room...");
            SendMessageToSpecialClient(client, "roomId=" + roomID);
            SendMessageToOther(client, "im connect to Rome :)");

        }

        public void ShareGameObject(Client client, GameObject gameObject)
        {
            //Console.WriteLine("gameObject: " + gameObject.ToString());
            SendMessageToAll(gameObject.ToString());
        }

        //SendMessage 
        public void SendMessageToAll(string message)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.sData);
            buffer.WriteString(message);

            foreach (Client item in clients)
                if (item.socket != null)
                    ServerTCP.SendDataTo(item.index, buffer.ToArray());

            buffer.Dispose();
        }
        public void SendMessageToOther(Client clientSource, string message)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.sData);
            buffer.WriteString(message);

            foreach (Client item in clients)
                if (item.index != clientSource.index && item.socket != null)
                {
                    Console.WriteLine("item.index: " + item.index);
                    ServerTCP.SendDataTo(item.index, buffer.ToArray());
                }

            buffer.Dispose();
        }
        public void SendMessageToSpecialClient(Client specialClient, string message)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.sData);
            buffer.WriteString(message);
            ServerTCP.SendDataTo(specialClient.index, buffer.ToArray());
            buffer.Dispose();
        }

    }
    
}
