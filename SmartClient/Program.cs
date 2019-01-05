using System;

namespace SmartClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientHandleNetworkData.InitializeNetworkPackages();
            ClientTCP.ConnectToServer();
            Console.ReadKey();
        }
    }
}
