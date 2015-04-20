using System;
using System.Runtime.Serialization;

namespace Roadplus.Server.EntityManagement
{
    [DataContract]
    public enum ActivityType
    {
        Unknown,
        [EnumMember(Value="get")]
        Get,
        [EnumMember(Value="set")]
        Set,
        [EnumMember(Value="create")]
        Create,
        [EnumMember(Value="remove")]
        Remove
    }
}

