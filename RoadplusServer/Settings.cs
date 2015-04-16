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
            // format: >FAIL:<reason>:;
            // summary: sends info about an error
            { "FAIL", CommandType.Failure },
            // format: >DISC:;
            // summary: graceful disconnect from any websocket client
            { "DISC", CommandType.Disconnect },

            { "SET", CommandType.Set },
            { "GET", CommandType.Get },
            { "CREATE", CommandType.Create },
            { "REMOVE", CommandType.Remove },
            { "EDIT", CommandType.Edit },

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

