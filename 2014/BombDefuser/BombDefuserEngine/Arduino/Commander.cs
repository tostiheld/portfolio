using System;
using CommandMessenger;
using CommandMessenger.TransportLayer;

namespace TowerHunterEngine.Arduino
{
    public class Commander : IDisposable
    {
        // recognized commands
        private enum Command
        {
            SetLed,
            Status
        }

        private SerialTransport Transport;
        private CmdMessenger Messenger;
        private TowerState receivedState;
        private TowerState newState;
        private bool refresh = false;

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
            Messenger.StartListening();
        }

        private void AttachCommandCallBacks()
        {
            Messenger.Attach(UnknownCommand);
            // TODO: attach more commands!
        }

        private void UnknownCommand(ReceivedCommand args)
        {
            throw new NotSupportedException(Properties.Resources.UnknownCommand);
        }

        public bool GetTowerState(ref TowerState state)
        {
            if (state == null)
                throw new ArgumentNullException();
            if (receivedState == null)
                throw new NullReferenceException();

            state = receivedState;
            return true;
        }

        public bool SetTowerState(TowerState state)
        {
            newState = state;
            refresh = true;
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
