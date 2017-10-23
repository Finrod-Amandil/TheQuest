using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TheQuest
{
    public partial class TheQuestForm : Form
    {
        private Game _game;
        private MovementMode _movementMode = MovementMode.Walk;
        
        public TheQuestForm()
        {
            _game = new Game(this);
            InitializeComponent();
            _game.NextLevel();
        }

        public void EnableControls()
        {
            foreach (Control control in Controls)
            {
                if (control is Button)
                {
                    control.Enabled = true;
                }
            }
        }

        public void DisableControls()
        {
            foreach (Control control in Controls)
            {
                if (control is Button)
                {
                    control.Enabled = false;
                }
            }
        }

        private void OnButtonMoveClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Direction direction = _movementButtons.FirstOrDefault(x => x.Value == button).Key;
            if (_movementMode == MovementMode.Walk)
            {
                try
                {
                    _game.Move(_game.Player, direction);
                }
                catch (BoardException be) { }
            }
            else
            {
                _game.Rotate(_game.Player, direction);
            }
        }

        private void OnButtonChangeMovementModeClick(object sender, EventArgs e)
        {
            if (_movementMode == MovementMode.Walk)
            {
                _movementMode = MovementMode.Turn;
                _buttonChangeMovementMode.BackColor = Color.Red;
            }
            else
            {
                _movementMode = MovementMode.Walk;
                _buttonChangeMovementMode.BackColor = Color.Lime;
            }
        }

        private void OnButtonUseItemClick(object sender, EventArgs e)
        {
            _game.UseItem(((InventoryIcon)sender).SlotNumber);
        }

        public void UpdateSprites()
        {
            List<Control> newControls = new List<Control>();

            //Update item sprites
            List<Item> items = new List<Item>();
            items.AddRange(_game.CurrentLevel.Items);

            foreach (Item item in items)
            {
                if (item.Field == null || !item.IsSpawned) continue;
                Image image = _itemPictures[item.GetType().ToString().Split(new char[] { '.' }).Last().ToLowerInvariant()];
                TransparentPictureBox tmp = new TransparentPictureBox()
                {
                    Image = image,
                    Location = new Point(
                        _startPoint.X + WALL_WIDTH + (item.Field.IndexX * UNIT_LENGTH) + (UNIT_LENGTH / 2) - (image.Width / 2),
                        _startPoint.Y + WALL_WIDTH + (item.Field.IndexY * UNIT_LENGTH) + (UNIT_LENGTH / 2) - (image.Height / 2)),
                    Size = new Size(image.Width, image.Height),
                    BackColor = Color.Transparent
                };
                newControls.Add(tmp);
            }

            //Update character sprites
            List<Character> characters = new List<Character>();
            characters.Add(_game.Player);
            characters.AddRange(_game.CurrentLevel.Enemies);

            foreach (Character character in characters)
            {
                if (character.Field == null || !character.IsSpawned) continue;
                Image image = new Bitmap(_characterPictures[character.GetType().ToString().Split(new char[] { '.' }).Last().ToLowerInvariant()]);

                switch (character.Orientation)
                {
                    case Direction.Up:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case Direction.Right:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case Direction.Down:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                        break;
                    case Direction.Left:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                }

                TransparentPictureBox tmp = new TransparentPictureBox()
                {
                    Image = image,
                    Location = new Point(
                        _startPoint.X + WALL_WIDTH + (character.Field.IndexX * UNIT_LENGTH) + (UNIT_LENGTH / 2) - (image.Width / 2),
                        _startPoint.Y + WALL_WIDTH + (character.Field.IndexY * UNIT_LENGTH) + (UNIT_LENGTH / 2) - (image.Height / 2)),
                    Size = new Size(image.Width, image.Height),
                    BackColor = Color.Transparent
                };

                newControls.Add(tmp);
                HealthBar healthBar = new HealthBar(tmp, character.HitPointsMax, character.HitPoints);
                newControls.AddRange(healthBar.GetControls());
            }

            //Update inventory icons
            Point scrollStart = new Point(
                _startPoint.X,
                _startPoint.Y + (WALL_WIDTH * 2) + (_roomSize.Height * UNIT_LENGTH));
            for (int i = 0; i < Game.INVENTORY_SIZE; i++)
            {
                InventorySlot currentSlot = _game.Player.Inventory.Slots[i];

                Image image;
                if (currentSlot.Item != null)
                {
                    image = _itemPictures[currentSlot.Item.GetType().ToString().Split(new char[] { '.' }).Last().ToLowerInvariant()];
                }
                else
                {
                    image = _itemPictures["empty"];
                }

                InventoryIcon tmp = new InventoryIcon()
                {
                    BackgroundImage = image,
                    Location = new Point(
                        scrollStart.X + SCROLL_HORZ_PAD + ((INV_SLOT_SIZE + INV_SLOT_PAD) * i) + ((INV_SLOT_SIZE - image.Width) / 2),
                        scrollStart.Y + SCROLL_VERT_PAD + ((INV_SLOT_SIZE - image.Height) / 2)),
                    Size = new Size(image.Width, image.Height),
                    SlotNumber = i,
                    Margin = new Padding(0)
                };

                if (currentSlot.Item != null)
                {
                    tmp.Visible = true;
                    tmp.Enabled = true;
                    tmp.Click += new EventHandler(OnButtonUseItemClick);
                }
                else
                {
                    tmp.Visible = false;
                    tmp.Enabled = false;
                }

                newControls.Add(tmp);
            }

            foreach (Control control in _sprites)
            {
                Controls.Remove(control);
            }
            _sprites.Clear();

            foreach (Control control in newControls)
            {
                _sprites.Add(control);
                Controls.Add(control);
                control.BringToFront();
            }
        }
    }
}
