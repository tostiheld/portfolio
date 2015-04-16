using System;
using System.Collections.Generic;
using Roadplus.Server.Communication;

namespace Roadplus.Server
{
	public partial class Server
	{
        private Dictionary<MessageTypes, Action<Message>> MessageCallbacks;

        private void AttachCallbacks()
        {
            MessageCallbacks = new Dictionary<MessageTypes, Action<Message>>()
            {
                { MessageTypes.Test, TestCallback },
                { MessageTypes.Identification, IdentificationCallback }
            };
        }
        
        private void TestCallback(Message message)
        {
            logStream.WriteLine("test");
        }

        private void IdentificationCallback(Message message)
        {
            logStream.WriteLine(message.MetaData);
            /*
            foreach (WSSession session in sessions)
            {
                if (session.IP == message.MessageSource.IP)
                {
                    session.SourceType = message.MessageSource.Type;
                }
            }*/
        }
	}
}
