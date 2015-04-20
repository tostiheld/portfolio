using System;

namespace Roadplus.Server.EntityManagement
{
    public class Entity
    {
        public string Name { get; private set; }

        public Entity(string name)
        {
            Name = name;
        }
    }
}

