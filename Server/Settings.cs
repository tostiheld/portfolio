using System;

namespace Server
{
    public struct Settings
    {
        public static string CarAssetName = "car.png";

        public static string RoadAssetName = "road.png";

        public static int RoadWidth = 32;

        public static Size CarSize = new Size(16, 24);

        public static int CarFOVMargin = CarSize.Height / 2;
    }
}

