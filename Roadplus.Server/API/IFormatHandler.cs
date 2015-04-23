using System;

namespace Roadplus.Server.API
{
    public interface IFormatHandler
    {
        string MessageFormat { get; }

        bool TryParse(RawMessage value, out Activity result);
        string Format(Response toformat);
    }
}

