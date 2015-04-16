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
            // format: >ACKN:;
            // summary: acknoledge (not used yet)
            { "ACKN", MessageTypes.Acknoledge },
            // format: >SOFF:;
            // summary: inform ui clients server goes offline
            { "SOFF", MessageTypes.ServerOffline },
            // format: >IDEN:<type>:;
            // summary: websocket client identifies itself for server
            //          <type> is UI or CAR
            { "IDEN", MessageTypes.Identification },
            // format: >GETS:<id>:;
            // summary: gets schools from zone with id <id>
            { "GETS", MessageTypes.GetSchools },
            // format: >CZON:<id>:<x>:<y>:;
            // summary: creates zone with id <id> at <x>, <y>
            { "CZON", MessageTypes.CreateZone },
            // format: >FAIL:<reason>:;
            // summary: sends info about an error
            { "FAIL", MessageTypes.Failure }
        };
    }
}

