using System;
using System.Threading;

namespace Roadplus.Server.Communication
{
    public abstract class Communicator
    {
        public const string MessageStart = ">";
        public const string MessageSplit = ":";
        public const string MessageTerminator = ";";

        public delegate void OnMessageReceived(object sender,MessageReceivedEventArgs e);
        public event OnMessageReceived MessageReceived;

        private string buffer;
        private Thread receiveThread;
        private bool stopListening;

        public Communicator()
        {
            receiveThread = new Thread(new ThreadStart(Receive));
        }

        /// <summary>
        /// Implements the way a child class should check if 
        /// the given channel is valid
        /// </summary>
        /// <returns>
        /// <c>true</c>, if channel was valid, <c>false</c> otherwise.
        /// </returns>
        /// <param name="channel">Where to connect to</param>
        protected abstract bool ValidateChannel(string channel);

        /// <summary>
        /// Gets a string from the underlying communication stream.
        /// This function should be non-blocking, as it 
        /// sits in a loop.
        /// </summary>
        /// <returns>The buffer.</returns>
        protected abstract string FillBuffer();

        /// <summary>
        /// Configures the connection before the listen thread
        /// starts.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the configuration succeeds, otherwise 
        /// <c>false</c></returns>
        protected abstract bool Configure();

        /// <summary>
        /// Send the specified message.
        /// </summary>
        /// <param name="message">Message.</param>
        public abstract void Send(Message message);

        /// <summary>
        /// Steps to be executed before the connection is 
        /// closed.
        /// </summary>
        protected virtual void BeforeClose()
        {

        }

        private void ProcessMessages()
        {
            int start = buffer.IndexOf(MessageStart);
            if (start != -1)
            {
                int end = buffer.IndexOf(MessageTerminator);
                if (end != -1)
                {
                    string msg = buffer.Substring(
                        start, (end - start) + 1);
                    buffer = buffer.Substring(end + 1);

                    string cmd = "";
                    string data = "";

                    Message received = Message.FromString(cmd, data);

                    if (Message != null &&
                        MessageReceived != null)
                    {
                        MessageReceivedEventArgs e = new MessageReceivedEventArgs(
                            received);
                        MessageReceived(this, received);
                    }
                }
            }
        }

        private void Receive()
        {
            while (!stopListening)
            {
                buffer += FillBuffer();
                ProcessMessages();
            }
        }

        public void StopReceiving()
        {
            BeforeClose();
            stopListening = true;
        }

        public void StartReceiving(string channel)
        {
            if (ValidateChannel(channel))
            {
                if (Configure())
                {
                    receiveThread.Start();
                    stopListening = false;
                }
                else
                {
                    throw new ConfigurationException();
                }
            }
            else
            {
                throw new InvalidChannelException();
            }
        }
    }
}

