using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;

using Roadplus.Server.API;
using Roadplus.Server.Data;

namespace Roadplus.Server.Commands.Json
{
    public class RemoveRoadConstructionCommand : ICommand
    {
        public string Name { get { return "removeRoadconstruction"; } }

        private const string idKey = "roadconstructionId";

        public IResponse Execute(string payload)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(payload);

            int id;

            if (!Int32.TryParse(o[idKey].ToString(), out id))
            {
                return new ResponseFailure(Name, "Parse error");
            }

            RoadplusData data = new RoadplusData();

            data.RoadConstructions.Where(rc => rc.RoadConstructionId == id).Delete();

            return new RemoveResponse()
            {
                Command = Name,
                RemovedObjectId = id
            };
        }

    }
}

