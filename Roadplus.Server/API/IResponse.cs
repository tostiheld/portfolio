using System;

namespace Roadplus.Server.API
{
    public interface IResponse
    {
        string ResponseString { get; }
        int ID { get; set; }
    }
}

