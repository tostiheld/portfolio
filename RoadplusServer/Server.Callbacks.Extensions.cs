using System;

using Roadplus.Server.Communication;
using Roadplus.Server.Map;

namespace Roadplus.Server
{
    public partial class Server
    {
        private void CreateZone(Message message)
        {
            int id = Convert.ToInt32(message.Payload[1]);
            int x = Convert.ToInt32(message.Payload[2]);
            int y = Convert.ToInt32(message.Payload[3]);

            if (GetZoneByID(id) != null)
            {
                TrySendFailure(
                    message.MessageSource.IP,
                    "Zone with id " + id.ToString() + " already exists");
                return;
            }

            Vertex start = new Vertex(new Point(x, y));
            Zone zone = new Zone(start, id);
            zones.Add(zone);

            WriteLineLog(
                String.Format(
                "Session at {0} created a zone with id {1}",
                message.MessageSource.IP.ToString(),
                id.ToString()));
            Message success = new Message(
                CommandType.Acknoledge,
                new string[] { id.ToString() },
            "zone");
            TrySendMessage(
                message.MessageSource.IP,
                success);
        }

        private void CreateSchool(Message message)
        {
            int id = Convert.ToInt32(message.Payload[1]);
            int sid = Convert.ToInt32(message.Payload[2]);
            string name = message.Payload[3];
            int hStart = Convert.ToInt32(message.Payload[4].Split('-')[0]);
            int mStart = Convert.ToInt32(message.Payload[4].Split('-')[1]);
            int hEnd = Convert.ToInt32(message.Payload[5].Split('-')[0]);
            int mEnd = Convert.ToInt32(message.Payload[5].Split('-')[1]);

            Zone target = GetZoneByID(id);
            if (target == null)
            {
                TrySendFailure(
                    message.MessageSource.IP,
                    "Zone with id " + id.ToString() + " not found");
                return;
            }

            if (target.GetSchoolByID(sid) != null)
            {
                TrySendFailure(
                    message.MessageSource.IP,
                    String.Format(
                    "School with id {0} in zone {1} already exists",
                    sid.ToString(),
                    id.ToString()));
                return;
            }

            DateTime sTime = new DateTime(1, 1, 1, hStart, mStart, 0);
            DateTime eTime = new DateTime(1, 1, 1, hEnd, mEnd, 0);
            School school = new School(
                new TimeRange(sTime, eTime),
                sid);
            school.Name = name;
            target.Schools.Add(school);

            WriteLineLog(
                String.Format(
                "Session at {0} added school with id {1} to zone {2}",
                message.MessageSource.IP.ToString(),
                sid.ToString(),
                id.ToString()));
            Message success = new Message(
                CommandType.Acknoledge,
                new string[] { sid.ToString() },
            "school");
            TrySendMessage(
                message.MessageSource.IP,
                success);
        }

        private void CreateRoadConstruction(Message message)
        {
            int id = Convert.ToInt32(message.Payload[1]);
            int rid = Convert.ToInt32(message.Payload[2]);
            string name = message.Payload[3];
            DateTime dstart = Convert.ToDateTime(message.Payload[4]);
            DateTime dend = Convert.ToDateTime(message.Payload[5]);

            Zone target = GetZoneByID(id);
            if (target == null)
            {
                TrySendFailure(
                    message.MessageSource.IP,
                    "Zone with id " + id.ToString() + " not found");
                return;
            }

            if (target.TEMPGetRCByID(rid) != null)
            {
                TrySendFailure(
                    message.MessageSource.IP,
                    String.Format(
                    "RoadConstruction with id {0} in zone {1} already exists",
                    rid.ToString(),
                    id.ToString()));
                return;
            }

            RoadConstruction r = new RoadConstruction(
                rid,
                name,
                new TimeRange(dstart, dend));
            target.RoadConstructions.Add(r);

            WriteLineLog(
                String.Format(
                "Session at {0} added roadconstruction with id {1} to zone {2}",
                message.MessageSource.IP.ToString(),
                rid.ToString(),
                id.ToString()));
            Message success = new Message(
                CommandType.Acknoledge,
                new string[] { rid.ToString() },
                "roadconstruction");
            TrySendMessage(
                message.MessageSource.IP,
                success);
        }
    }
}

