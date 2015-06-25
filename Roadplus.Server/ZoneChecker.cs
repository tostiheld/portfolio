using System;
using System.Timers;

using Roadplus.Server.Communication;
using Roadplus.Server.Messages.Text;

namespace Roadplus.Server
{
    public class ZoneChecker
    {
        private RoadLink Connection;
        private Timer checkTimer;

        public ZoneChecker(RoadLink connection, int interval)
        {
            if (connection == null)
            {
                throw new ArgumentNullException();
            }

            Connection = connection;
            Connection.Disconnected += Connection_Disconnected;
            checkTimer = new Timer(interval * 1000);
            checkTimer.Elapsed += CheckTimer_Elapsed;
            checkTimer.Start();
        }

        void CheckTimer_Elapsed (object sender, ElapsedEventArgs e)
        {
            Connection.Send(new TemperatureRequest());
            Connection.Send(new TrafficDensityRequest());
        }

        void Connection_Disconnected (object sender, EventArgs e)
        {
            checkTimer.Stop();
        }
    }
}

