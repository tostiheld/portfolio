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

        public readonly static Dictionary<string, CommandType> Messages = 
        new Dictionary<string, CommandType>()
        {
            // format: >ACKN:;
            // summary: acknoledge (not used yet)
            { "ACKN", CommandType.Acknoledge },
            // format: >SOFF:;
            // summary: inform ui clients server goes offline
            { "SOFF", CommandType.ServerOffline },
            // format: >IDEN:<type>:;
            // summary: websocket client identifies itself for server
            //          <type> is UI or CAR
            { "IDEN", CommandType.Identification },
            // format: >GETS:<id>:;
            // summary: gets schools from zone with id <id>
            { "GETS", CommandType.GetSchools },
            // format: >CZON:<id>:<x>:<y>:;
            // summary: creates zone with id <id> at <x>, <y>
            { "CZON", CommandType.CreateZone },
            // format: >RZON:<id>:;
            // summary: removes zone with id <id>
            { "RZON", CommandType.RemoveZone },
            // format: >FAIL:<reason>:;
            // summary: sends info about an error
            { "FAIL", CommandType.Failure },
            // format: >DISC:;
            // summary: graceful disconnect from any websocket client
            { "DISC", CommandType.Disconnect },
            // format: >GRDS:;
            // summary: get all available serial devices
            { "GRDS", CommandType.GetRoads },
            // format: >CONR:<id>:<port>:;
            // summary: connects the road device on port <port> 
            //          with the zone with id <id>
            { "CONR", CommandType.ConnectRoadToZone },
            // format: >SIGN:<speed>:;
            // summary: sets road sign at the specified speed
            // or if from websocket:
            // format: >SIGN:<speed>:id:;
            // summary sets road sign in zone
            { "SIGN", CommandType.SetRoadSign },
            // format: >TEMP:;
            // summary: gets temp from road
            { "TEMP", CommandType.Temperature }
        };
    }
}

