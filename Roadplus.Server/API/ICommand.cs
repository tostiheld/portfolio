using System;

namespace Roadplus.Server.API
{
    public interface ICommand
    {
        string Name { get; }
        IResponse Execute(string payload);
    }
}

