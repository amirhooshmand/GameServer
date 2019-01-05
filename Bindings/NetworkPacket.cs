using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    //get send from server to client;
    //client has no listent to ServerPacket
    public enum ServerPackets
    {
        sConnectionOK = 1,
    }

    //get send from client to server;
    //server has to listent to ClientPacket
    public enum ClientPackets
    {
        cThankYou = 1,
    }
}