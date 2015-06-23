using System;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class CommandProcessorText : CommandProcessor
    {
        #region implemented abstract members of CommandProcessor

        public override IResponse Process(string command)
        {
            throw new NotImplementedException();
        }

        #endregion

        public CommandProcessorText()
        {
        }
    }
}

