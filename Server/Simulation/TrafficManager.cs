using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server.Simulation
{
    public class TrafficManager : DrawableGameComponent
    {
        private List<Road> Network;
        private SortedList<int, Car> Cars;
        private int CarsPointer;

        private SpriteBatch spriteBatch;
        private Engine Parent;
        private Point Endpoint;

        private Texture2D CarTexture;

        public TrafficManager(Engine parent, Road start)
            : base(parent)
        {
            Network = new List<Road>();
            Cars = new SortedList<int, Car>();

            Parent = parent;
            spriteBatch = Parent.spriteBatch;

            Endpoint = start.Start;
            AddRoad(start);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            CarTexture = Parent.Content.Load<Texture2D>(
                Settings.CarAssetName);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Car c in Cars.Values)
            {

            }
            /*
            foreach (Car x in Cars)
            {
                foreach (Car y in Cars)
                {
                    if (x.FOV.Intersects(
                        y.FOV))
                    {
                        float delta = Convert.ToSingle(
                            x.FOV.Top - y.FOV.Bottom);
                        x.Slowrate = 1 / delta;
                    }
                }
            }*/
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            /*
            foreach (Car c in Cars)
            {
                c.Draw(spriteBatch);
            }*/
        }

        public void AddCar()
        {
            Car c = new Car(
                CarTexture,
                Settings.CarSize);

            Cars.Add(CarsPointer, c);
            CarsPointer++;
        }

        public void RemoveCar()
        {
            if (Cars.Count > 1)
            {
                Car target = Cars.Values[CarsPointer];
                Cars.Remove(CarsPointer);
                target.Dispose();
                CarsPointer--;
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
    }
}

