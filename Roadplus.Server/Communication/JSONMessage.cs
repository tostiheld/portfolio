using System;
using System.Runtime.Serialization;

using Roadplus.Server.EntityManagement;

namespace Roadplus.Server.Communication
{
    [DataContract]
    public class JSONMessage
    {
        [DataMember(Name="type")]
        public ActivityType Type { get; set; }
        [DataMember(Name="parameters")]
        public string[] Parameters { get; set; }

        public JSONMessage() { }
    }
}

