using System;
using System.Linq;

using Roadplus.Server.API;
using Roadplus.Server.Data;
using System.Collections.Generic;

namespace Roadplus.Server.Messages.Json
{
    public class GetSchoolsCommand : ICommand
    {
        public string Name { get { return "requestSchools"; } }

        public IResponse Execute(string payload)
        {
            RoadplusData data = new RoadplusData();

            School[] zones = data.Schools.ToList().ToArray();

            return new GetResponse()
            {
                Command = Name,
                RequestedObjects = zones
            };
        }
    }
}

