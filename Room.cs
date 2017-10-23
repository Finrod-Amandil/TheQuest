using System.Collections.Generic;

namespace TheQuest
{
    internal class Room
    {
        private const int MIN_ROOM_WIDTH = 10;
        private const int MIN_ROOM_HEIGHT = 4;

        private int _width;
        private int _height;
        private List<WallFeature> _features;

        public int Width { get { return _width; } }

        public int Height { get { return _height; } }

        public List<WallFeature> WallFeatures { get; set; }

        public Room(int width, int height)
        {
            _width = width < MIN_ROOM_WIDTH ? MIN_ROOM_WIDTH : width;
            _height = height < MIN_ROOM_HEIGHT ? MIN_ROOM_HEIGHT : height;
        }
    }
}
