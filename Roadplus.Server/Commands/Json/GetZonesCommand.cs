using System;
using System.Linq;

using Roadplus.Server.API;
using Roadplus.Server.Data;
using System.Collections.Generic;

namespace Roadplus.Server.Commands.Json
{
    public class GetZonesCommand : ICommand
    {
        public IResponse Execute(string payload)
        {
            RoadplusData data = new RoadplusData();

            Zone[] zones = data.Zones.ToList().ToArray();

            GetResponse response = new GetResponse();
            response.RequestedObjects = zones;

            return response;
        }

        public string Name
        {
            get
            {
                return "requestZones";
            }
        }
    }
}

