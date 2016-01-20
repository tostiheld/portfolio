using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BombDefuserEngine.Utils.Exceptions
{
    class EV3CommunicationException : Exception
    {
        public EV3CommunicationException() 
        {
            this.Message = "There was an error communicating with the EV3.";
        }
        public EV3CommunicationException( string message ) : base( message ) 
        {
            this.Message = "There was an error communicating with the EV3. " + message;
        }
        public EV3CommunicationException( string message, Exception inner ) : base( message, inner )
        {
            this.Message = "There was an error communicating with the EV3. " + message;
            this.InnerException = inner;
        }

        public string Message { get; private set; }
        public Exception InnerException { get; private set; }
    }
}
