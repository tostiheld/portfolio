using System;
using CommandMessenger;
using CommandMessenger.TransportLayer;

namespace TowerHunterEngine.Arduino
{
    public class Commander : IDisposable
    {
        private SerialTransport Transport;
        private CmdMessenger Messenger;

        public Commander(String portname, int baudrate, BoardType boardtype)
        {
            Transport = new SerialTransport
            {
                CurrentSerialSettings =
                { 
                    PortName = portname, 
                    BaudRate = baudrate, 
                    DtrEnable = false
                }
            };

            Messenger = new CmdMessenger(Transport)
            {
                BoardType = boardtype
            };

            AttachCommandCallBacks();
            Messenger.NewLineReceived += Messenger_NewLineReceived;
            Messenger.NewLineSent += Messenger_NewLineSent;
            Messenger.StartListening();
        }

        private void Messenger_NewLineSent(object sender, NewLineEvent.NewLineArgs e)
        {
            // TODO: handle event
            throw new NotImplementedException();
        }

        private void Messenger_NewLineReceived(object sender, NewLineEvent.NewLineArgs e)
        {
            // TODO: handle event
            throw new NotImplementedException();
        }

        private void AttachCommandCallBacks()
        {
            // TODO: attach commands to methods
            throw new NotImplementedException();
        }

        public TowerState GetTowerState()
        {
            TowerState state = new TowerState();
            // TODO: receive and store state of the towers
            return state;
        }

        public bool SetTowerState(TowerState state)
        {
            // TODO: set state of the towers
            return true;
        }

        public void Stop()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Messenger.StopListening();
                Messenger.Dispose();
                Transport.Dispose();
            }
        }
    }
}
