using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerHunterEngine.Utils
{
    public static class Messages
    {
        public const string FieldEmpty = "Can not perform a generation on a field object which is null";
        public const string FieldGenerated = "Can not perform a generation on a field object which is already generated";
        public const string UnknownCommand = "Received command is unknown";
    }
}
