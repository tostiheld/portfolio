using System;
using System.Collections.Generic;

using Roadplus.Server.Communication;

namespace Roadplus.Server
{
    public struct Settings
    {
        public const int VertexRadius = 5;

        public readonly static Dictionary<string, MessageTypes> Messages = 
        new Dictionary<string, MessageTypes>()
        {
            { "TEST", MessageTypes.Test }
        };

        // We cant have too many threads!
        public const int MaxCarConnections = 100;
        public const int CarReceiveBufferSize = 256;
    }
}

