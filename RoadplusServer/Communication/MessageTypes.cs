using System;

namespace Roadplus.Server.Communication
{
    public enum MessageTypes
    {
        Acknoledge,
        Failure,
        Disconnect,
        ServerOffline,

        SetRoadSign,
        Temperature,

        Identification,
        GetRoads,
        ConnectRoadToZone,
        GetSchools,
        CreateZone,
        RemoveZone
    }
}

