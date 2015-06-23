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
            return new GetRoadsResponse()
            {
                Ports = Source.ConnectedPorts.ToArray()
            };
        }
    }
}

