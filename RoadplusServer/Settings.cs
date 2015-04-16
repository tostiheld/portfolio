using System;
using System.IO;
using System.Reflection;
using System.Net;
using System.Collections.Generic;

using Roadplus.Server.Communication;

namespace Roadplus.Server
{
    public static class Settings
    {
        public static readonly IPAddress IP = IPAddress.Any;

        public const int Port = 42424;

        public const int BaudRate = 38400;

        public const string ZoneFileName = "default.zones";

        public static readonly string ZoneFilePath = Path.Combine(
            Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location), 
            ZoneFileName);

        public const int BufferSize = 256;

        public const int VertexRadius = 5;

        public readonly static Dictionary<string, MessageTypes> Messages = 
        new Dictionary<string, MessageTypes>()
        {
            { "ACKN", MessageTypes.Acknoledge },
            { "TEST", MessageTypes.Test },
            { "SOFF", MessageTypes.ServerOffline },
            { "IDEN", MessageTypes.Identification },
            { "GETS", MessageTypes.GetSchools },
            { "CZON", MessageTypes.CreateZone }
        };
    }
}

