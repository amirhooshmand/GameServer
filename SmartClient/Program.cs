using System;

namespace SmartClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientHandleNetworkData.InitializeNetworkPackages();
            ClientTCP.ConnectToServer();

            A:

            ConsoleKeyInfo key = Console.ReadKey();

            if (key.KeyChar == 'g')
            {
                ClientTCP.SendGameobject();
            }
            else if (key.KeyChar == 'j')
            {
                ClientTCP.JoinToRoom();
            }

            goto A;
        }
    }
}
