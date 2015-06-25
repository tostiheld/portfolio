using System;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Text
{
    public class TemperatureMessage : ICommand
    {
        public string Name
        {
            get
            {
                return "TEMP";
            }
        }

        public IResponse Execute(string payload)
        {
            Console.WriteLine("test");
            return null;
        }

    }
}

