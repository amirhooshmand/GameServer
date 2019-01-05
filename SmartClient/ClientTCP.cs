using System;
using System.Net;
using System.Net.Sockets;
using Bindings;


namespace SmartClient
{
    class ClientTCP
    {
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private byte[] asyncBuffer = new byte[1024];

        public static void ConnectToServer()
        {
            Console.WriteLine("Connecting to server...");
            clientSocket.BeginConnect("127.0.0.1", 5555, new AsyncCallback(ConnectCallback), clientSocket);
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            clientSocket.EndConnect(ar);
            while (true)
            {
                OnRecevie();
            }
        }
        private static void OnRecevie()
        {
            byte[] sizeInfo = new byte[4];
            byte[] receviedBuffer = new byte[1024];

            int totalRead = 0, currentRead = 0;

            try
            {
                currentRead = totalRead = clientSocket.Receive(sizeInfo);
                if (totalRead <= 0)
                {
                    Console.WriteLine("You are not connect to server...");
                }
                else
                {
                    while (totalRead < sizeInfo.Length && currentRead > 0)
                    {
                        currentRead = clientSocket.Receive(sizeInfo, totalRead, sizeInfo.Length - totalRead, SocketFlags.None);
                        totalRead += currentRead;
                    }

                    int messageSize = 0;
                    messageSize |= (sizeInfo[0]);
                    messageSize |= (sizeInfo[1] << 8);
                    messageSize |= (sizeInfo[2] << 16);
                    messageSize |= (sizeInfo[3] << 24);

                    byte[] data = new byte[messageSize];
                    totalRead = 0;
                    currentRead = totalRead = clientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);

                    while (totalRead < messageSize && currentRead > 0)
                    {
                        currentRead = clientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);
                        totalRead += currentRead;
                    }

                    //HandleNetworkInformation
                    ClientHandleNetworkData.HandleNetworkInformation(data);
                }
            }
            catch
            {
                Console.WriteLine("You are not connect to server.");
            }
        }

        public static void SendData(byte[] data)
        {
            clientSocket.Send(data);
        }

        public static void ThankYouServer()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ClientPackets.cThankYou);
            buffer.WriteString("Than bruv, for letting me to connect ya server");
            SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendGameobject()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ClientPackets.cShareObject);
            buffer.WriteString("{ \"roomId\": 0, \"gameObject\": { \"id\": \"46546\", \"name\": \"N\", \"positionX\": 12.2, \"positionY\": 12.2, \"positionZ\": 12.2, \"rotationX\": 12.2, \"rotationY\": 12.2, \"rotationZ\": 12.2 } }");
            SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void JoinToRoom()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ClientPackets.cJoinRoom);
            SendData(buffer.ToArray());
            buffer.Dispose();
        }
    }
}


