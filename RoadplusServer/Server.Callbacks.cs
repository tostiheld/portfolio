using System;
using System.Collections.Generic;
using Roadplus.Server.Communication;

namespace Roadplus.Server
{
	public partial class Server
	{
        private readonly Dictionary<MessageTypes, Action<string>> MessageCallbacks;

        private void AttachCallbacks()
        {
            MessageCallbacks = new Dictionary<MessageTypes, Action<string>>()
            {
                { MessageTypes.Test, new Action<string>(TestCallback) }
            };
        }
        
        private void TestCallback(string data)
        {
            logStream.WriteLine("test");
        }
	}
}
