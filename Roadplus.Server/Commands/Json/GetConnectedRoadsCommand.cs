using System;

using Roadplus.Server.API;
using Roadplus.Server.Communication;
using System.Collections.Generic;

namespace Roadplus.Server.Commands.Json
{
    public class GetConnectedRoadsCommand : ICommand
    {
        public string Name { get { return "getRoads"; } }

        private RoadLinkManager Source;

        public GetConnectedRoadsCommand(RoadLinkManager source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            Source = source;
        }

        public IResponse Execute(string payload)
        {
            List<string> ports = new List<string>();

            foreach (Link l in Source.Links)
            {
                ports.Add(l.Address);
            }

            return new GetRoadsResponse()
            {
                Ports = ports.ToArray()
            };
        }
    }
}

