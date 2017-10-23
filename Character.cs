using System.Linq;

namespace TheQuest
{
    abstract internal class Character : IMoveable
    {
        protected string _name;
        private Field _field;
        private Direction _orientation;
        private int _hitPoints;
        private int _hitPointsMax;
        protected bool _isAlive;
        private bool _isSpawned = false;

        public Character(int hitPoints)
        {
            _orientation = Direction.Down;
            _hitPoints = hitPoints;
            _hitPointsMax = hitPoints;
            _isAlive = true;
        }

        public Field Field
        {
            get { return _field; }
            set { _field = value; }
        }
        public Direction Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }
        public int HitPoints
        {
            get { return _hitPoints; }
            set { _hitPoints = value; }
        }
        public int HitPointsMax
        {
            get { return _hitPointsMax; }
            set { _hitPointsMax = value; }
        }
        public bool IsAlive
        {
            get { return _isAlive; }
        }
        public bool IsSpawned
        {
            get { return _isSpawned; }
            set { _isSpawned = value; }
        }

        public string Name
        {
            get
            {
                return _name == null ? GetType().ToString().Split(new char[] { '.' }).Last() : _name;
            }
            set { _name = value; }
        }

        /// <summary>
        /// Updates the character's field to the given field
        /// </summary>
        /// <param name="field">The field to move the character to.</param>
        public void MoveTo(Field field)
        {
            _field = field;
        }

        /// <summary>
        /// Subtracts the amount of damage given of the character's hitpoints. Makes the character faint if hitpoints reach 0.
        /// </summary>
        /// <param name="damage"></param>
        public virtual void TakeDamage(int damage)
        {
            _hitPoints -= damage;
            if (_hitPoints <= 0)
            {
                _hitPoints = 0;
                Faint();
            }
        }

        private void Faint()
        {
            _isSpawned = false;
            _isAlive = false;
        }
    }
}
