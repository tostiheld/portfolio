using System;

namespace Roadplus.Server.Communication
{
    public enum CommandType
    {
        Acknoledge,
        Failure,
        Disconnect,
        ServerOffline,

        Identification,
        Set,
        Get,
        Create,
        Remove,
        Edit,

        SetRoadSign,
        Temperature,

        Unknown = 9999
    }
}

