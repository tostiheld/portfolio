using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Roadplus.Server.Communication
{
    public class CarListener
    {
        private List<CarCommunicator> clients;

        private TcpListener tcpListener;
        private IPAddress IP;
        private int Port;

        private Thread listenThread;
        private volatile bool stopListening;

        public CarListener(string ip, int port)
        {
            if (!IPAddress.TryParse(ip, out IP))
            {
                throw new ArgumentException("Invalid IP");
            }

            Port = port;

            clients = new List<CarCommunicator>();
            listenThread = new Thread(new ThreadStart(Listen));
        }

        private void Listen()
        {
            tcpListener = new TcpListener(IP, Port);

            try
            {
                tcpListener.Start(10);
            }
            catch (SocketException ex)
            {
                switch (ex.ErrorCode)
                {
                    // TODO: what codes do we want to handle?
                    // eg case SocketError.AccessDenied:
                    default:
                        throw;
                }
            }

            while (!stopListening)
            {
                if (clients.Count <= Settings.MaxCarConnections)
                {
                    try
                    {
                        if (!tcpListener.Pending())
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        Socket client = tcpListener.AcceptSocket();
                        CarCommunicator car = new CarCommunicator(client);
                        // MAGIC
                        // copied from working code. why is this needed?
                        RemoveClient(car.IP);
                    }
                    catch (Exception ex)
                    {
                        // TODO: make better
                        throw ex;
                    }
                }
            }

            tcpListener.Stop();
        }

        private void clientM_Disconnected(object sender, EventArgs e)
        {
            // does this work? lol
            ClientManager target = (ClientManager)sender;
            RemoveClient(target.IP);

            OnClientDisconnect(
                new ClientEventArgs(target));
        }

        private void clientM_DataReceived(object sender, ReceivedEventArgs e)
        {
            // bubble up directly for now
            OnDataReceived(e);
        }

        private int FindClient(IPAddress ip)
        {
            int index = 0;
            foreach (ClientManager c in clients)
            {
                if (c.IP.Equals(ip))
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        public void Start()
        {
            stopListening = false;
            listenThread.Start();
        }

        public void Stop()
        {
            stopListening = true;

            listenThread.Join();

            foreach (ClientManager c in clients)
            {
                c.Disconnect();
            }
        }

        public bool RemoveClient(IPAddress ip)
        {
            lock (this)
            {
                int index = FindClient(ip);

                if (index >= 0)
                {
                    string ipString = clients[index].IP.ToString();
                    clients.RemoveAt(index);

                    // broadcast disconnection

                    return true;
                }

                return false;
            }
        }
    }
}

