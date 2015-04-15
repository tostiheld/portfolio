/*using System;

namespace Roadplus.Server
{
    public class TrafficSimulation
    {
        private List<Road> Network;
        private Dictionary<int, Car> Cars;
        private int CarsPointer;

        private Engine Parent;
        private SpriteBatch spriteBatch;
        private Point Endpoint;

        private Texture2D CarTexture;
        private Texture2D RoadTexture;

        private bool disposed;

        public TrafficManager(Engine parent, Road start)
            : base(parent)
        {
            Network = new List<Road>();
            Cars = new Dictionary<int, Car>();

            Parent = parent;

            Endpoint = start.Start;
            AddRoad(start);
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            CarTexture = Parent.Content.Load<Texture2D>(
                Settings.CarAssetName);

            RoadTexture = Parent.Content.Load<Texture2D>(
                Settings.RoadAssetName);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0, j = 1; i < Cars.Count; i++)
            {
                if (j <= Cars.Count)
                {
                    Car x = Cars[i];
                    Car y = Cars[j];

                    if (x.FOV.Intersects(
                        y.FOV))
                    {
                        float delta = Convert.ToSingle(
                            x.FOV.Top - y.FOV.Bottom);
                        x.Slowrate = 1 / delta;
                    }
                }

                foreach (Road r in Network)
                {
                    //Cars[i].Update(gameTime, );
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            foreach (Car c in Cars.Values)
            {
                c.Draw(
                    spriteBatch,
                    CarTexture);
            }

            foreach (Road r in Network)
            {
                r.Draw(
                    spriteBatch,
                    RoadTexture);
            }

            spriteBatch.End();
        }

        public void AddCar()
        {
            Car c = new Car(
                Settings.CarSize);

            Cars.Add(CarsPointer, c);
            CarsPointer++;
        }

        public void RemoveCar()
        {
            if (Cars.Count > 1)
            {
                Car target = Cars[CarsPointer];
                Cars.Remove(CarsPointer);
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

        public void AddRoad(Point p)
        {
            Road r = new Road(
                Endpoint,
                p,
                Settings.RoadWidth);

            Network.Add(r);
            Endpoint = r.End;
        }

        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);           
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return; 

            if (disposing) {
                // free managed
                CarTexture.Dispose();
                RoadTexture.Dispose();
            }

            // free unmanaged

            disposed = true;
        }

        ~TrafficManager()
        {
            Dispose(false);
        }
    }
}*/
