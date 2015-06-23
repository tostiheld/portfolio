using System;

namespace Roadplus.Server.API
{
    public interface IResponse
    {
        string ResponseString { get; }
        string Command { get; set; }
        string ToString();
    }
}

