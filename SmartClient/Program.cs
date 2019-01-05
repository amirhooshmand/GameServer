using System;

namespace SmartClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientHandleNetworkData.InitiakizeNetworkPackages();
            ClientTCP.ConnectToServer();
            Console.ReadKey();
        }
    }
}
