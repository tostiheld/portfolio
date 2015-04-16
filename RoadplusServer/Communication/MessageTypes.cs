using System;

namespace Roadplus.Server.Communication
{
    public enum MessageTypes
    {
        Acknoledge,
        Failure,
        Disconnect,
        ServerOffline,
        Identification,
        GetRoads,
        ConnectRoadToZone,
        GetSchools,
        CreateZone
    }
}

