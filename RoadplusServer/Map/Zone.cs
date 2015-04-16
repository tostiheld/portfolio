using System;
using System.Collections.Generic;
using ProtoBuf;
using Roadplus.Server.Communication;

namespace Roadplus.Server.Map
{
    [ProtoContract]
    public class Zone
    {
        [ProtoMember(5)]
        public int ID { get; private set; }
        [ProtoMember(6)]
        public string Name { get; set; }
        //[ProtoMember(7)]
        public List<School> Schools { get; private set; }
        public List<RoadConstruction> RoadConstructions { get; private set; }
        //[ProtoMember(8)]
        public RoadCommunication Road 
        { 
            get
            {
                return road;
            }
            set
            {
                road = value;
                road.MessageReceived += Road_MessageReceived;
                road.Start();
            }
        }

        private RoadCommunication road;

        [ProtoMember(1)]
        private Vertex root;
        [ProtoMember(2)]
        private Vertex radarLocation;
        [ProtoMember(3)]
        private List<Vertex> vertices;
        [ProtoMember(4)]
        private List<Edge> edges;

        // USED BY PROTBUF-NET
        // DO NOT EXPOSE
        private Zone() { }

        public Zone(Vertex startingPoint, int id)
        {
			if (startingPoint == null) {
				throw new ArgumentNullException ("startingPoint");
			}
            vertices = new List<Vertex>();
            edges = new List<Edge>();

            ID = id;
            Schools = new List<School>();

            RoadConstructions = new List<RoadConstruction>();

            root = startingPoint;
            vertices.Add(root);
            root.NewConnection += Vertex_NewConnection;
        }

        private void Road_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Received.ToString());
        }

        private void Vertex_NewConnection(object sender, NewConnectionEventArgs e)
        {
            e.End.NewConnection += Vertex_NewConnection;

            vertices.Add(e.End);
            edges.Add(e.Connection);
        }

        /// <summary>
        /// Convert a location in pixels to a vertex, using the 
        /// Vertex tolerance radius as defined in the Settings
        /// </summary>
        /// <returns>A vertex if the location was within the tolerance</returns>
        /// <param name="point">The location to check.</param>
        public Vertex PointToVertex(Point point)
        {
            foreach (Vertex v in vertices)
            {
                // formula to check if point is in circle
                // (x - center_x)^2 + (y - center_y)^2 < radius^2
                if (((point.X - v.Location.X) * (point.X - v.Location.X) +
                     (point.Y - v.Location.Y) * (point.Y - v.Location.Y)) <
                     Settings.VertexRadius * Settings.VertexRadius)
                {
                    return v;
                }

            }

            return null;
        }

        /// <summary>
        /// Connect to a real life road
        /// </summary>
        /// <param name="comms">The communicator to pair with</param>
        /// <param name="location">The location of the radar on the map</param> 
        public void Connect(RoadCommunication comms, Point location)
        {
			if (comms == null) {
				throw new ArgumentNullException ("comms");
			}
            Vertex radarlocation = PointToVertex(location);
            if (radarlocation == null)
            {
                throw new VertexNotFoundException();
            }
            else
            {
                radarLocation = radarlocation;

                // further implementation down here
            }
        }

        public RoadConstruction TEMPGetRCByID(int id)
        {
            foreach (RoadConstruction r in RoadConstructions)
            {
                if (r.ID == id)
                {
                    return r;
                }
            }

            return null;
        }

        public School GetSchoolByID(int id)
        {
            foreach (School s in Schools)
            {
                if (s.ID == id)
                {
                    return s;
                }
            }

            return null;
        }

        public void SetSign(int speed)
        {
            Message message = new Message(
                CommandType.SetRoadSign,
                new string[] { speed.ToString() });
            Road.Send(message);
        }

        public void GetTemp()
        {
            Message message = new Message(
                CommandType.Temperature);
            Road.Send(message);
        }
    }
}

