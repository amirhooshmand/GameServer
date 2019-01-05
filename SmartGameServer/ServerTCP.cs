using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Bindings;

namespace SmartGameServer
{
    class ServerTCP
    {
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] buffer = new byte[1024];

        public static Client[] clients = new Client[Constants.MAX_PLAYER];
        public static List<Room> rooms = new List<Room>();
        public static void SetupServer()
        {
            for (int i = 0; i < Constants.MAX_PLAYER; i++)
                clients[i] = new Client();

            Console.WriteLine("Setup server...");

            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 5555));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = serverSocket.EndAccept(ar);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            for (int i = 0; i < Constants.MAX_PLAYER; i++)
            {
                if (clients[i].socket == null)
                {
                    Console.WriteLine("user id: " + i);
                    clients[i].socket = socket;
                    clients[i].index = i;
                    clients[i].ip = socket.RemoteEndPoint.ToString();
                    clients[i].StartClient();
                    Console.WriteLine("Connection from '{0}' reviced", clients[i].ip);
                    SendConnectionOK(i);

                    return;
                }
            }
        }

        public static void SendDataTo(int index, byte[] data)

        {
            byte[] sizeInfo = new byte[4];
            sizeInfo[0] = (byte)data.Length;
            sizeInfo[1] = (byte)(data.Length >> 8);
            sizeInfo[2] = (byte)(data.Length >> 16);
            sizeInfo[3] = (byte)(data.Length >> 24);

            clients[index].socket.Send(sizeInfo);
            clients[index].socket.Send(data);


        }

        public static void SendConnectionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.sConnectionOK);
            buffer.WriteString("you successfully connect to serevr");
            SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

    }
}
