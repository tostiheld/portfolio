using System;

namespace Roadplus.Server.EntityManagement
{
    public class EntityManager : AutoIncrementingDictionary<Entity>
    {
        private MessageExchange messageExchange;

        public EntityManager(MessageExchange exchange)
            : base()
        {
            messageExchange = exchange;
            exchange.NewActivity += Exchange_NewActivity;
        }

        private void Exchange_NewActivity(object sender, NewActivityEventArgs e)
        {

        }

        public void Load(string path)
        {

        }

        public void Save(string path)
        {

        }
    }
}

