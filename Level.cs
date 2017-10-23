using System.Collections.Generic;
using System.Drawing;

namespace TheQuest
{
    internal class Level
    {
        private Board _board;
        private Room _room;
        private List<Enemy> _enemies;
        private List<Item> _items;

        public Level(int levelNumber)
        {
            SetLevelSettings(levelNumber);
            _board.SetFieldAttributes(_room.WallFeatures);
        }

        public Board Board
        {
            get { return _board; }
        }

        public Room Room
        {
            get { return _room; }
        }

        public List<Enemy> Enemies
        {
            get { return _enemies; }
        }

        public List<Item> Items
        {
            get { return _items; }
        }

        private void SetLevelSettings(int levelNumber)
        {
            switch (levelNumber)
            {
                case 1:
                    _room = new Room(1, 1);
                    _board = new Board(new Size(_room.Width, _room.Height));
                    _enemies = new List<Enemy>()
                    {
                        new Bat(),
                        new Ghost()
                    };
                    _items = new List<Item>()
                    {
                        new Axe(),
                    };
                    _room.WallFeatures = new List<WallFeature>()
                    {
                        new Door(Direction.Up, 5),
                        new Portal(Direction.Right, 1),
                        new Portal(Direction.Down, 3),
                        new Painting(Direction.Up, 7)
                    };
                    break;
                case 2:
                    _room = new Room(20, 6);
                    _board = new Board(new Size(_room.Width, _room.Height));
                    _enemies = new List<Enemy>()
                    {
                        new Bat(),
                        new Bat(),
                        new Bat(),
                    };
                    _items = new List<Item>()
                    {
                        new Mace(),
                    };
                    _room.WallFeatures = new List<WallFeature>()
                    {
                        new Door(Direction.Left, 4),
                        new Portal(Direction.Right, 1),
                        new Portal(Direction.Down, 3),
                        new Portal(Direction.Up, 7),
                        new Painting(Direction.Left, 2),
                        new Painting(Direction.Down, 6)
                    };
                    break;
                case 3:
                    _room = new Room(10, 10);
                    _board = new Board(new Size(_room.Width, _room.Height));
                    _enemies = new List<Enemy>()
                    {
                        new Ghoul(),
                        new Ghoul(),
                        new Wizard(),
                        new Bat(),
                    };
                    _items = new List<Item>()
                    {
                        new Bow(),
                        new HealingPotion(),
                        new HealingPotion(),
                    };
                    _room.WallFeatures = new List<WallFeature>()
                    {
                        new Door(Direction.Left, 4),
                        new Portal(Direction.Right, 4),
                        new Painting(Direction.Up, 6),
                        new FireBowl(Direction.Right, 6),
                        new FireBowl(Direction.Right, 2)
                    };
                    break;
                case 4:
                    _room = new Room(10, 4);
                    _board = new Board(new Size(_room.Width, _room.Height));
                    _enemies = new List<Enemy>()
                    {
                        new Ghost(),
                        new Ghost(),
                        new Ghost(),
                        new Ghoul(),
                    };
                    _items = new List<Item>()
                    {
                        new HealingPotion(),
                    };
                    _room.WallFeatures = new List<WallFeature>()
                    {
                        new Door(Direction.Down, 1),
                        new Portal(Direction.Right, 1),
                        new Painting(Direction.Left, 2),
                        new FireBowl(Direction.Down, 7),
                        new FireBowl(Direction.Up, 7)
                    };
                    break;
            }
        }
    }
}
