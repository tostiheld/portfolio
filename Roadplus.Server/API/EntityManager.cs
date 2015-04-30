using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Roadplus.Server.API
{
    public class EntityManager<T>
    {
        protected AutoIncrementingDictionary<T> Entities;

        private MessageExchange messageExchange;
        private Type TargetType;

        public EntityManager(MessageExchange exchange)
            : base()
        {
            TargetType = typeof(T);
            messageExchange = exchange;
            exchange.NewActivity += Exchange_NewActivity;
        }

        private void Exchange_NewActivity(object sender, NewActivityEventArgs e)
        {
            if (e.NewActivity.TargetTypes.Contains(TargetType))
            {
                Response response = new Response(
                    e.NewActivity.Type,
                    e.NewActivity.SourceAddress);

                switch(e.NewActivity.Type)
                {
                    case ActivityType.Create:
                        HandleCreate(e.NewActivity, ref response);
                        break;
                    case ActivityType.Get:
                        HandleGet(e.NewActivity, ref response);
                        break;
                    case ActivityType.Remove:
                        HandleRemove(e.NewActivity, ref response);
                        break;
                    default:
                        // nuthin to do here
                        // wieieieieieie
                        break;
                }

                messageExchange.Post(response);
            }
        }

        private void HandleRemove(Activity activity, ref Response response)
        {
            foreach (KeyValuePair<Type, object> pair in activity.Payload)
            {
                if (pair.Key == TargetType &&
                    pair.Value is Int32)
                {
                    int index = Convert.ToInt32(pair.Value);
                    Entities.RemoveAt(index);
                }
            }

            response.Type = ResponseType.Acknoledge;
            response.Message = "Entity removed";
        }

        private void HandleGet(Activity activity, ref Response response)
        {
            response.Type = ResponseType.Acknoledge;

            foreach (KeyValuePair<Type, object> pair in activity.Payload)
            {
                if (pair.Key == TargetType)
                {
                    if (pair.Value.ToString() == "all")
                    {
                        // TODO: wth is this?
                        response.PayloadList.AddRange(
                            Entities.ToList() as List<object>);
                        // probably the best thing to do now...
                        return;
                    }
                    else if (pair.Value is Int32)
                    {
                        int index = Convert.ToInt32(pair.Value);
                        T result;
                        if (Entities.TryGetValue(index, out result))
                        {
                            response.PayloadList.Add(result);
                        }
                    }
                    else
                    {
                        response.Type = ResponseType.Failure;
                        response.Message = "Invalid entity id encountered";
                        return;
                    }
                }
            }
        }

        private void HandleCreate(Activity activity, ref Response response)
        {
            foreach (KeyValuePair<Type, object> pair in activity.Payload)
            {
                if (pair.Key == TargetType)
                {
                    try
                    {
                        int result = Entities.Add((T)pair.Value);
                        JObject newEntity = new JObject(
                            result,
                            (T)pair.Value);
                        response.PayloadList.Add(newEntity);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        response.Type = ResponseType.Failure;
                        response.Message = "Database index too big";
                    }
                }
            }

            response.Type = ResponseType.Acknoledge;
        }


        public void Load(string path)
        {

        }

        public void Save(string path)
        {

        }
    }
}

