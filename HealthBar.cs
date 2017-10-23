using System.Drawing;
using System.Windows.Forms;

namespace TheQuest
{
    internal class HealthBar
    {
        private const int WIDTH = 36;
        private const int HEIGHT = 3;
        private const int OFFSET_VERT = -10;

        private Label _container;
        private Label _healthBar;

        public HealthBar(Control parent, int hitPointsMax, int hitPoints)
        {
            Point location = new Point(parent.Location.X + (parent.Size.Width / 2) - (WIDTH / 2),
                                       parent.Location.Y + parent.Size.Height + OFFSET_VERT);

            _container = new Label()
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Transparent,
                Location = new Point(location.X - 1, location.Y - 1),
                Size = new Size(WIDTH + 2, HEIGHT + 2)
            };

            _healthBar = new Label()
            {
                BorderStyle = BorderStyle.None,
                BackColor = GetColor(hitPointsMax, hitPoints),
                Location = location,
                Size = new Size((int)(WIDTH * ((double)hitPoints / (double)hitPointsMax)), HEIGHT)
            };
            if (_healthBar.Size.Width == 0)
            {
                _healthBar.Size = new Size(1, _healthBar.Size.Height);
                _healthBar.Visible = false;
            }
        }

        public Point Location
        {
            get { return _container.Location; }
            set
            {
                _container.Location = value;
                _healthBar.Location = value;
            }
        }

        private Color GetColor(int hitPointsMax, int hitPoints)
        {
            double ratio = ((double)hitPoints) / ((double)hitPointsMax);

            return ratio < 0.25D ? Color.Red :
                   ratio < 0.5D  ? Color.Gold :
                                   Color.Lime;
        }

        /// <summary>
        /// Returns all the controls the Healthbar consists of
        /// </summary>
        /// <returns>Array of controls containing the controls of the healthbar.</returns>
        public Control[] GetControls()
        {
            Control[] controls = new Control[2];
            controls[0] = _container;
            controls[1] = _healthBar;
            return controls;
        }
    }
}
