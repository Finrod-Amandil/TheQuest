using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TheQuest
{
    public partial class TheQuestForm
    {
        private const int WALL_WIDTH = 75;
        private const int UNIT_LENGTH = 40;
        private const int SCROLL_HEIGHT = 100;
        private const int SCROLL_HORZ_PAD = 75;
        private const int SCROLL_VERT_PAD = 15;
        private const int INV_SLOT_SIZE = 60;
        private const int INV_SLOT_PAD = 5;
        private const int BUTTON_SIZE = INV_SLOT_SIZE / 3;
        private const int BUTTON_PAD = 1;

        private Size _roomSize;

        private List<PictureBox> _wall;
        private List<List<PictureBox>> _floor;
        private List<PictureBox> _wallFeatures;
        private List<Control> _sprites;

        private Point _startPoint;

        private Dictionary<string, Image> _backgroundPictures;
        private Dictionary<string, Image> _characterPictures;
        private Dictionary<string, Image> _itemPictures;

        private Dictionary<Direction, Button> _movementButtons;
        private Button _buttonChangeMovementMode;

        public void BuildRoom(Point startPoint)
        {
            _startPoint = startPoint;
            _roomSize = new Size(_game.CurrentLevel.Room.Width, _game.CurrentLevel.Room.Height);
            this.ClientSize = new Size(
                _startPoint.X + WALL_WIDTH * 2 + _roomSize.Width * UNIT_LENGTH,
                _startPoint.Y + WALL_WIDTH * 2 + _roomSize.Height * UNIT_LENGTH + SCROLL_HEIGHT);
            Controls.Clear();
            LoadPictures();
            BuildWall();
            BuildFloor();
            BuildScroll();
            PlaceWallFeatures();
            BuildInventory();
            BuildControls();
            _sprites = new List<Control>();
        }

        private void LoadPictures()
        {
            _backgroundPictures = new Dictionary<string, Image>();
            _characterPictures = new Dictionary<string, Image>();
            _itemPictures = new Dictionary<string, Image>();

            Dictionary<string, Image> currentPictures;

            foreach (string dir in Directory.GetDirectories(@".\..\..\Resources\img\"))
            {
                string subdir = dir.Split(new char[] { '\\' }).Last();
                currentPictures = subdir == "background" ? _backgroundPictures :
                                  subdir == "character" ? _characterPictures :
                                  subdir == "item" ? _itemPictures :
                                  new Dictionary<string, Image>();

                foreach (string file in Directory.GetFiles($@".\img\{subdir}\"))
                {
                    if (file.Split(new char[] { '.' }).Last() == "png")
                    {
                        string fileName = file.Split(new char[] { '\\' }).Last();
                        currentPictures.Add(
                            fileName.Split(new char[] { '.' }).First(),
                            Image.FromFile(file));
                    }
                }
            }
        }

        private void BuildWall()
        {
            PictureBox tmp;
            Point lastLocation = new Point(_startPoint.X + WALL_WIDTH - UNIT_LENGTH, _startPoint.Y);
            _wall = new List<PictureBox>();
            RotateFlipType rotation = RotateFlipType.RotateNoneFlipNone;
            int segmentCount = 0;
            int directionModifierX = 0;
            int directionModifierY = 0;

            for (int wall = 0; wall < 4; wall++)
            {
                switch (wall)
                {
                    case 0: //top wall
                        rotation = RotateFlipType.RotateNoneFlipNone;
                        segmentCount = _roomSize.Width;
                        directionModifierX = 1;
                        directionModifierY = 0;
                        break;
                    case 1: //right wall
                        rotation = RotateFlipType.Rotate90FlipNone;
                        segmentCount = _roomSize.Height;
                        directionModifierX = 0;
                        directionModifierY = 1;
                        break;
                    case 2: //bottom wall
                        rotation = RotateFlipType.Rotate180FlipNone;
                        segmentCount = _roomSize.Width;
                        directionModifierX = -1;
                        directionModifierY = 0;
                        break;
                    case 3: //left wall
                        rotation = RotateFlipType.Rotate270FlipNone;
                        segmentCount = _roomSize.Height;
                        directionModifierX = 0;
                        directionModifierY = -1;
                        break;
                }

                //Straight wall
                for (int segment = 0; segment < segmentCount; segment++)
                {
                    tmp = new PictureBox
                    {
                        Image = new Bitmap(_backgroundPictures["wall"]),
                        Location = new Point(
                            lastLocation.X + (directionModifierX * UNIT_LENGTH),
                            lastLocation.Y + (directionModifierY * UNIT_LENGTH)),
                        Size = directionModifierX == 0 ?
                               new Size(WALL_WIDTH, UNIT_LENGTH) :
                               new Size(UNIT_LENGTH, WALL_WIDTH)
                    };
                    tmp.Image.RotateFlip(rotation);

                    //correction for right wall
                    if (wall == 1 && segment == 0)
                    {
                        tmp.Location = new Point(tmp.Location.X, tmp.Location.Y + WALL_WIDTH - UNIT_LENGTH);
                    }

                    _wall.Add(tmp);
                    lastLocation = _wall.Last().Location;
                }

                //Corner
                tmp = new PictureBox
                {
                    Image = new Bitmap(_backgroundPictures["corner"]),
                    Location = new Point(
                            lastLocation.X + (directionModifierX * UNIT_LENGTH),
                            lastLocation.Y + (directionModifierY * UNIT_LENGTH)),
                    Size = new Size(WALL_WIDTH, WALL_WIDTH)
                };
                tmp.Image.RotateFlip(rotation);
                tmp.Image.RotateFlip(RotateFlipType.Rotate90FlipNone); //rotate an additional 90°

                //correction for bottom-left corner
                if (wall == 2)
                {
                    tmp.Location = new Point(tmp.Location.X - (WALL_WIDTH - UNIT_LENGTH), tmp.Location.Y);
                }
                //correction for top-left corner
                if (wall == 3)
                {
                    tmp.Location = new Point(tmp.Location.X, tmp.Location.Y - (WALL_WIDTH - UNIT_LENGTH));
                }

                _wall.Add(tmp);
                lastLocation = _wall.Last().Location;
            }

            foreach (PictureBox pic in _wall)
            {
                Controls.Add(pic);
            }
        }

        private void BuildFloor()
        {
            _floor = new List<List<PictureBox>>();
            for (int horz = 0; horz < _roomSize.Width; horz++)
            {
                _floor.Add(new List<PictureBox>());
                for (int vert = 0; vert < _roomSize.Height; vert++)
                {
                    PictureBox tmp = new PictureBox()
                    {
                        Image = new Bitmap(_backgroundPictures["floor"]),
                        Location = new Point(
                            _startPoint.X + WALL_WIDTH + (horz * UNIT_LENGTH),
                            _startPoint.Y + WALL_WIDTH + (vert * UNIT_LENGTH)),
                            Size = new Size(UNIT_LENGTH, UNIT_LENGTH)
                    };
                    _floor[horz].Add(tmp);
                    Controls.Add(tmp);
                }
            }
        }

        private void BuildScroll()
        {
            Point scrollStart = new Point(
                _startPoint.X,
                _startPoint.Y + (WALL_WIDTH * 2) + (_roomSize.Height * UNIT_LENGTH));

            PictureBox tmp = new PictureBox()
            {
                Image = new Bitmap(_backgroundPictures["scroll_end_l"]),
                Location = scrollStart,
                Size = new Size(WALL_WIDTH, SCROLL_HEIGHT)
            };
            Controls.Add(tmp);

            Random random = new Random(); //RNG for scroll sections
            int scrollElementsAvailable = 0;
            foreach(string key in _backgroundPictures.Keys)
            {
                if (key.Contains("scroll")) scrollElementsAvailable++;
            }
            scrollElementsAvailable -= 2; //Subtract left and right end

            for (int horz = 0; horz < _roomSize.Width * 2; horz++)
            {
                tmp = new PictureBox()
                {
                    Image = new Bitmap(_backgroundPictures["scroll_" + random.Next(scrollElementsAvailable)]),
                    Location = new Point(scrollStart.X + WALL_WIDTH + (horz * (UNIT_LENGTH / 2)), scrollStart.Y),
                    Size = new Size(UNIT_LENGTH / 2, SCROLL_HEIGHT)
                };
                Controls.Add(tmp);
            }
            tmp = new PictureBox()
            {
                Image = new Bitmap(_backgroundPictures["scroll_end_r"]),
                Location = new Point(
                    scrollStart.X + WALL_WIDTH + (_roomSize.Width * UNIT_LENGTH),
                    scrollStart.Y),
                Size = new Size(WALL_WIDTH, SCROLL_HEIGHT)
            };
            Controls.Add(tmp);
        }

        private void PlaceWallFeatures()
        {
            _wallFeatures = new List<PictureBox>();
            foreach (WallFeature feature in _game.CurrentLevel.Room.WallFeatures)
            {
                if (feature is WallFeature)
                {
                    Bitmap image = new Bitmap(_backgroundPictures[feature.GetType().ToString().Split(new char[] { '.' }).Last().ToLowerInvariant()]);
                    PictureBox tmp = null;
                    switch (feature.Wall)
                    {
                        case Direction.Up:
                            image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                            tmp = new PictureBox()
                            {
                                Image = image,
                                Location = new Point(
                                    _startPoint.X + WALL_WIDTH + (feature.Position * UNIT_LENGTH) + (UNIT_LENGTH / 2) - (image.Width / 2),
                                    _startPoint.Y),
                                Size = new Size(image.Width, image.Height)
                            };
                            break;
                        case Direction.Right:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tmp = new PictureBox()
                            {
                                Image = image,
                                Location = new Point(
                                    _startPoint.X + WALL_WIDTH + (_roomSize.Width * UNIT_LENGTH),
                                    _startPoint.Y + WALL_WIDTH + (feature.Position * UNIT_LENGTH) + (UNIT_LENGTH / 2) - (image.Height / 2)),
                                Size = new Size(image.Width, image.Height)
                            };
                            break;
                        case Direction.Down:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tmp = new PictureBox()
                            {
                                Image = image,
                                Location = new Point(
                                    _startPoint.X + WALL_WIDTH + (_roomSize.Width * UNIT_LENGTH) - ((feature.Position * UNIT_LENGTH) + (UNIT_LENGTH / 2) + (image.Width / 2)),
                                    _startPoint.Y + WALL_WIDTH + (_roomSize.Height * UNIT_LENGTH)),
                                Size = new Size(image.Width, image.Height)
                            };
                            break;
                        case Direction.Left:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tmp = new PictureBox()
                            {
                                Image = image,
                                Location = new Point(
                                    _startPoint.X,
                                    _startPoint.Y + WALL_WIDTH + (_roomSize.Height * UNIT_LENGTH) - ((feature.Position * UNIT_LENGTH) + (UNIT_LENGTH / 2) + (image.Height / 2))),
                                Size = new Size(image.Width, image.Height)
                            };
                            break;
                    }
                    _wallFeatures.Add(tmp);
                }
            }
            foreach (PictureBox pic in _wallFeatures)
            {
                Controls.Add(pic);
                Controls[Controls.Count - 1].BringToFront();
            }
        }

        private void BuildInventory()
        {
            Point scrollStart = new Point(
                _startPoint.X,
                _startPoint.Y + (WALL_WIDTH * 2) + (_roomSize.Height * UNIT_LENGTH));

            for (int i = 0; i < Game.INVENTORY_SIZE; i++)
            {
                Label tmp = new Label()
                {
                    BackColor = Color.Silver,
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(scrollStart.X + SCROLL_HORZ_PAD + (i * (INV_SLOT_SIZE + INV_SLOT_PAD)), scrollStart.Y + SCROLL_VERT_PAD),
                    Size = new Size(INV_SLOT_SIZE, INV_SLOT_SIZE)
                };
                Controls.Add(tmp);
                Controls[Controls.Count - 1].BringToFront();
            }
        }

        private void BuildControls()
        {
            Point scrollStart = new Point(
                _startPoint.X,
                _startPoint.Y + (WALL_WIDTH * 2) + (_roomSize.Height * UNIT_LENGTH));
            _movementButtons = new Dictionary<Direction, Button>();

            //Button Up
            Button tmp = new Button()
            {
                Location = new Point(
                    ClientSize.Width - SCROLL_HORZ_PAD - INV_SLOT_SIZE + BUTTON_SIZE + BUTTON_PAD,
                    scrollStart.Y + SCROLL_VERT_PAD - BUTTON_PAD),
                Size = new Size(BUTTON_SIZE, BUTTON_SIZE),
                Text = "⮝"
            };
            _movementButtons.Add(Direction.Up, tmp);

            //Button Right
            tmp = new Button()
            {
                Location = new Point(
                    ClientSize.Width - SCROLL_HORZ_PAD - INV_SLOT_SIZE + (2 * (BUTTON_SIZE + BUTTON_PAD)),
                    scrollStart.Y + SCROLL_VERT_PAD + BUTTON_SIZE),
                Size = new Size(BUTTON_SIZE, BUTTON_SIZE),
                Text = "⮞"
            };
            _movementButtons.Add(Direction.Right, tmp);

            //Button Down
            tmp = new Button()
            {
                Location = new Point(
                    ClientSize.Width - SCROLL_HORZ_PAD - INV_SLOT_SIZE + BUTTON_SIZE + BUTTON_PAD,
                    scrollStart.Y + SCROLL_VERT_PAD + (2 * BUTTON_SIZE) + BUTTON_PAD),
                Size = new Size(BUTTON_SIZE, BUTTON_SIZE),
                Text = "⮟"
            };
            _movementButtons.Add(Direction.Down, tmp);

            //Button Left
            tmp = new Button()
            {
                Location = new Point(
                    ClientSize.Width - SCROLL_HORZ_PAD - INV_SLOT_SIZE,
                    scrollStart.Y + SCROLL_VERT_PAD + BUTTON_SIZE),
                Size = new Size(BUTTON_SIZE, BUTTON_SIZE),
                Text = "⮜"
            };
            _movementButtons.Add(Direction.Left, tmp);

            //Button for movement mode
            _buttonChangeMovementMode = new Button()
            {
                Location = new Point(
                    ClientSize.Width - SCROLL_HORZ_PAD - INV_SLOT_SIZE + BUTTON_SIZE + BUTTON_PAD,
                    scrollStart.Y + SCROLL_VERT_PAD + BUTTON_SIZE),
                Size = new Size(BUTTON_SIZE, BUTTON_SIZE),
                Text = "⭮",
                BackColor = Color.Lime
            };
            _movementMode = MovementMode.Walk;
            _buttonChangeMovementMode.Click += new EventHandler(OnButtonChangeMovementModeClick);

            foreach (Direction direction in _movementButtons.Keys)
            {
                _movementButtons[direction].Click += new EventHandler(OnButtonMoveClick);
                Controls.Add(_movementButtons[direction]);
                Controls[Controls.Count - 1].BringToFront();
            }

            Controls.Add(_buttonChangeMovementMode);
            Controls[Controls.Count - 1].BringToFront();
        }
    }
}
