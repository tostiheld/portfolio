using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server.Simulation
{
    public class TrafficManager : DrawableGameComponent
    {
        private List<Road> Network;
        private Queue<Car> Cars;

        private SpriteBatch spriteBatch;
        private Engine Parent;
        private Point Endpoint;

        public TrafficManager(Engine parent, Road start)
            : base(parent)
        {
            Network = new List<Road>();
            Cars = new Queue<Car>();

            spriteBatch = parent.spriteBatch;

            Endpoint = start.Start;
            AddRoad(start);
        }

        public void AddCar()
        {
            Car c = new Car(
                Parent.Content, 
                Settings.CarAssetName);

            Cars.Enqueue(c);
        }

        public void RemoveCar()
        {
            if (Cars.Count > 1)
            {
                Car target = Cars.Dequeue();
                target.Dispose();
            }
        }

        public void AddRoad(Road road)
        {
            if (road == null)
            {
                throw new ArgumentNullException("road");
            }
            else if (Endpoint.X != road.Start.X &&
                     Endpoint.Y != road.Start.Y)
            {
                throw new UnmatchingStartpointException();
            }

            Network.Add(road);
            Endpoint = road.End;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Car c in Cars)
            {
                c.Update();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (Car c in Cars)
            {
                c.Draw(spriteBatch);
            }
        }
    }
}

