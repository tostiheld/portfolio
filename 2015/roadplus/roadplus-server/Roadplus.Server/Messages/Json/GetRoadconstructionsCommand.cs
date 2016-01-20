using System;
using System.Linq;

using Roadplus.Server.API;
using Roadplus.Server.Data;
using System.Collections.Generic;

namespace Roadplus.Server.Messages.Json
{
    public class GetRoadconstructionsCommand : ICommand
    {
        public string Name { get { return "requestRoadconstructions"; } }

        public IResponse Execute(string payload)
        {
            RoadplusData data = new RoadplusData();

            RoadConstruction[] zones = data.RoadConstructions.ToList().ToArray();

            return new GetResponse()
            {
                Command = Name,
                RequestedObjects = zones
            };
        }
    }
}

