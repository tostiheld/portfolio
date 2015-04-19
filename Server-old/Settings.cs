using System;
using System.IO;
using System.Reflection;
using System.Net;
using System.Collections.Generic;

using Roadplus.Server.Communication;
using Roadplus.Server.Communication.Protocol;

namespace Roadplus.Server
{
    public static class Settings
    {
        public static IPAddress IP = IPAddress.Any;

        public static int Port = 42424;

        public static bool EnableHttp = true;

        public static bool LogToFile = false;

        public static string HttpPort = "8080";

        public static string HttpServiceUrl = "http://" + IP.ToString() + ":" + HttpPort + "/";

        public static int BaudRate = 38400;

        public static string FileRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string HttpRoot = Path.Combine(FileRoot, "www");

        public static string ZoneFileName = "default.zones";

        public static string ZoneFilePath = Path.Combine(
                FileRoot, 
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
            // format: >arduino:<port>:<baudrate>:<command>:<payload>:;
            // summar: arduino override
            { "ARDUINO", CommandType.RoadDirect },

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

