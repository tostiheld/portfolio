using System;
using System.Linq;

using Roadplus.Server.API;
using Roadplus.Server.Data;
using System.Collections.Generic;

namespace Roadplus.Server.Commands.Json
{
    public class GetZonesCommand : ICommand
    {
        public string Name { get { return "requestZones"; } }

        public IResponse Execute(string payload)
        {
            RoadplusData data = new RoadplusData();

            Zone[] zones = data.Zones.ToList().ToArray();

            return new GetResponse()
            {
                Command = Name,
                RequestedObjects = zones
            };
        }
    }
}

