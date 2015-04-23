using System;
using System.Runtime.Serialization;

namespace Roadplus.Server.API
{
    public enum ActivityType
    {
        Unknown,
        Identify,
        Get,
        Set,
        Create,
        Remove
    }
}

