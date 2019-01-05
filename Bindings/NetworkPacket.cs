namespace Bindings
{
    //get/send from server to client;
    //client has to listent to ServerPacket
    public enum ServerPackets
    {
        sConnectionOK = 1,
        sData = 2,
    }

    //get send from client to server;
    //server has to listent to ClientPacket
    public enum ClientPackets
    {
        cThankYou = 1,
        cData = 2,
        cJoinRoom = 3,
        cShareObject = 4,
    }
}