using System;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Text
{
    public class DensityMessage : ICommand
    {
        public IResponse Execute(string payload)
        {
            return null;
        }

        public string Name
        {
            get
            {
                return "dens";
            }
        }
    }
}

