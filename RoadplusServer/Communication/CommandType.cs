using System;

namespace Roadplus.Server.Communication
{
    public enum CommandType
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

