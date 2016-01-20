using System;

namespace Roadplus.Server.API
{
    public interface IFormatter
    {
        string Format(IResponse toformat);
        string Format(IRequest toformat);
    }
}

