using System;

namespace Roadplus.Server.API
{
    public interface IRequest
    {
        string Command { get; }
        string[] Payload { get; }
    }
}

