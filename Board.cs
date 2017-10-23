using System;
using System.Collections.Generic;
using System.Drawing;

namespace TheQuest
{
    internal class Board
    {
        private List<List<Field>> _fields;
        private Size _size;

        public Board(Size size)
        {
            _size = size;
            _fields = new List<List<Field>>();

            //Initialise fields
            for (int x = 0; x < _size.Width; x++)
            {
                _fields.Add(new List<Field>());
                for (int y = 0; y < _size.Height; y++)
                {
                    _fields[x].Add(new Field(x, y));
                }
            }
        }
        public Size Size {
            get { return _size; }
        }
        public List<List<Field>> Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// Iterates through all Wall Features and sets the attribute of each field in front of a wall feature.
        /// </summary>
        /// <param name="wallFeatures">List of WallFeatures</param>
        public void SetFieldAttributes(List<WallFeature> wallFeatures)
        {
            foreach(WallFeature feature in wallFeatures)
            {
                Field affectedField = feature.Wall == Direction.Up    ? GetField(feature.Position, 0) :
                                      feature.Wall == Direction.Right ? GetField(_size.Width - 1, feature.Position) :
                                      feature.Wall == Direction.Down  ? GetField(_size.Width - 1 - feature.Position, _size.Height - 1) :
                                                                        GetField(0, _size.Height - 1 - feature.Position);

                affectedField.Attribute = feature.Attribute;
            }
        }

        /// <summary>
        /// Moves a Character or Item into a specified direction. Throws BoardException if no movement was possible.
        /// Throws no exception if only partial movement was possible.
        /// </summary>
        /// <param name="thingToMove">The object to move</param>
        /// <param name="direction">The direction to move the object along</param>
        /// <param name="steps">The number of fields the object should be moved</param>
        public void Move(IMoveable thingToMove, Direction direction, int steps)
        {
            if (thingToMove is Item)
            {
                Move(thingToMove as Item, direction, steps);
            }
            else if (thingToMove is Character)
            {
                Move(thingToMove as Character, direction, steps);
            }
        }

        public void Move(IMoveable thingToMove, Direction direction)
        {
            Move(thingToMove, direction, 1);
        }

        /// <summary>
        /// Moves an Item on the Board. Items can be moved over fields where another Item 
        /// or Character is, and can be placed on fields where a character is, 
        /// but not on fields where another item is.
        /// Throws BoardException if no movement was possible at all. Partial movement does not throw an exception.
        /// </summary>
        /// <param name="itemToMove">The item to move</param>
        /// <param name="direction">The direction to move the object along</param>
        /// <param name="steps">The number of fields the object should be moved</param>
        public void Move(Item itemToMove, Direction direction, int steps)
        {
            Field currentField = itemToMove.Field;
            //Proceed field by field until all steps have been taken or end of board is reached.
            for (int i = 0; i < steps; i++)
            {
                try
                {
                    currentField = GetNextField(currentField, direction);
                }
                catch (BoardException be)
                {
                    //If first field in path is already outside board, no movement is possible. Abort and throw exception.
                    if (i == 0)
                    {
                        throw be;
                    }
                    //Else move as far as possible and do not throw an exception
                    else break;
                }
            }

            //Check what is placed on the field, and place item if possible
            if (currentField.Item != null)   //cannot place two items on the same field
            {
                throw new FieldOccupiedException();
            }
            else
            {
                itemToMove.MoveTo(currentField); //Update the item's location
            }
        }

        /// <summary>
        /// Moves a character across the board. The character can not proceed if another 
        /// character is in the way or the edge of the board is reached. Can walk over 
        /// items. Items that are walked over are not picked up, only items on the final 
        /// field are, and only if the character is a player and only if "pickUpThings" is true.
        /// Throws MovementException if no movement was possible at all. Partial movement does not throw an exception.
        /// </summary>
        /// <param name="characterToMove">The character to move</param>
        /// <param name="direction">The direction to move the object along</param>
        /// <param name="steps">The number of fields the object should be moved</param>
        /// <param name="pickUpThings">Whether the character should pick up things (only possible for 
        /// character type = Player)</param>
        public void Move(Character characterToMove, Direction direction, int steps, bool pickUpThings)
        {
            Field currentField = characterToMove.Field;
            //Proceed field by field until all steps have been taken or end of board is reached.
            for (int i = 0; i < steps; i++)
            {
                try
                {
                    currentField = GetNextField(currentField, direction);
                    if (currentField.Character != null)
                    {
                        throw new FieldOccupiedException();
                    }
                }
                catch (BoardException mve)
                {
                    if (i == 0)
                    {
                        throw mve;
                    }
                    else
                    {
                        break;
                    }
                }
                characterToMove.Field = currentField;
            }

            //Consider picking up an item
            if (currentField.Item != null && characterToMove is Player && pickUpThings == true)
            {
                Player player = characterToMove as Player;
                player.PickUpItem(currentField.Item, out bool hasPickedUp);
                if (hasPickedUp)
                {
                    currentField.Item.Field = null;
                }
            }
        }

        /// <summary>
        /// Moves a character across the board. The character can not proceed if another 
        /// character is in the way or the edge of the board is reached. Can walk over 
        /// items. If the final field has an item in it it will be picked up, if the character is a Player
        /// Throws MovementException if no movement was possible at all. Partial movement does not throw an exception.
        /// </summary>
        /// <param name="characterToMove">The character to move</param>
        /// <param name="direction">The direction to move the object along</param>
        /// <param name="steps">The number of fields the object should be moved</param>
        public void Move(Character characterToMove, Direction direction, int steps)
        {
            Move(characterToMove, direction, steps, true);
        }

        /// <summary>
        /// Checks all field's attributes and return those fields which are suitable of spawning the requested object
        /// </summary>
        /// <param name="objectToSpawn">The object to spawn</param>
        /// <returns>List of all fields that can spawn the object</returns>
        public List<Field> GetSpawnPoints(IPlaceable objectToSpawn)
        {
            List<Field> spawnPoints = new List<Field>();
            foreach(List<Field> column in _fields)
            {
                foreach (Field field in column) 
                {
                    if ((objectToSpawn is Enemy  && field.Attribute == FieldAttribute.EnemySpawn)
                     || (objectToSpawn is Player && field.Attribute == FieldAttribute.PlayerSpawn)
                     ||  objectToSpawn is Item)
                    {
                        spawnPoints.Add(field);
                    }
                }
            }
            return spawnPoints;
        }

        private Field GetNextField(Field currentField, Direction direction)
        {
            int indexX = currentField.IndexX;
            int indexY = currentField.IndexY;

            switch (direction)
            {
                case Direction.Up :
                    indexY--;
                    break;
                case Direction.Right:
                    indexX++;
                    break;
                case Direction.Down:
                    indexY++;
                    break;
                case Direction.Left:
                    indexX--;
                    break;
            }

            if (indexY < 0 || indexX < 0 || indexY >= _size.Height || indexX >= _size.Width)
            {
                throw new OutsideBoardException();
            }

            return _fields[indexX][indexY];
        }

        /// <summary>
        /// Returns the field at specific coordinates of the board.
        /// Throws OutsideBoardException of coordinates are out of valid range.
        /// </summary>
        /// <param name="indexX">X-coordinate of the field</param>
        /// <param name="indexY">>-coordinate of the field</param>
        /// <returns>The field at the position</returns>
        public Field GetField(int indexX, int indexY)
        {
            if (indexY < 0 || indexX < 0 || indexY >= _size.Height || indexX >= _size.Width)
            {
                throw new OutsideBoardException();
            }
            else
            {
                return _fields[indexX][indexY];
            }
        }

        /// <summary>
        /// Calculates the walking distance (not geometrical distance) between two objects on the board
        /// </summary>
        /// <param name="object1">The first object</param>
        /// <param name="object2">The second object</param>
        /// <returns>Distance in fields between the objects</returns>
        public int GetDistanceBetween(IPlaceable object1, IPlaceable object2)
        {
            return Math.Abs(object1.Field.IndexX - object2.Field.IndexX) +
                   Math.Abs(object1.Field.IndexY - object2.Field.IndexY);
        }

        /// <summary>
        /// Calculates in which direction (out of Up, Right, Down, Left) an object lies seen from another object.
        /// If two directions are equally accurate, a random one of the two is given.
        /// </summary>
        /// <param name="viewer">Object to orient</param>
        /// <param name="target">Object to orient to</param>
        /// <returns>Direction of the target as seen from the viewer</returns>
        public Direction GetDirectionTowards(IPlaceable viewer, IPlaceable target)
        {
            int dX = viewer.Field.IndexX - target.Field.IndexX;
            int dY = viewer.Field.IndexY - target.Field.IndexY;

            if (Math.Abs(dX) > Math.Abs(dY))
            {
                return dX < 0 ? Direction.Right : Direction.Left;
            }
            else if (Math.Abs(dX) < Math.Abs(dY))
            {
                return dY < 0 ? Direction.Down : Direction.Up;
            }
            else
            {
                Random random = new Random();
                if (random.Next(2) == 0) return dX < 0 ? Direction.Right : Direction.Left;
                else return dY < 0 ? Direction.Down : Direction.Up;
            }
        }

        /// <summary>
        /// Returns the character that's on the field pointed to by a specific target. Returns null if no character is on that field.
        /// </summary>
        /// <param name="attacker">The character from which the target is seen</param>
        /// <param name="target">A target object representing the relative distance from the attacker to the target character</param>
        /// <returns>The character on the targeted field, or null if that field is empty or outside the board.</returns>
        public Character GetTargetCharacter(Character attacker, Target target)
        {
            int targetFieldIndexX = 0;
            int targetFieldIndexY = 0;
            switch(attacker.Orientation)
            {
                case Direction.Up:
                    targetFieldIndexX = attacker.Field.IndexX + target.TranslationPerpendicular;
                    targetFieldIndexY = attacker.Field.IndexY - target.TranslationStraight;
                    break;

                case Direction.Right:
                    targetFieldIndexX = attacker.Field.IndexX + target.TranslationStraight;
                    targetFieldIndexY = attacker.Field.IndexY + target.TranslationPerpendicular;
                    break;

                case Direction.Down:
                    targetFieldIndexX = attacker.Field.IndexX - target.TranslationPerpendicular;
                    targetFieldIndexY = attacker.Field.IndexY + target.TranslationStraight;
                    break;

                case Direction.Left:
                    targetFieldIndexX = attacker.Field.IndexX - target.TranslationStraight;
                    targetFieldIndexY = attacker.Field.IndexY - target.TranslationPerpendicular;
                    break;
            }

            if (targetFieldIndexX >= _size.Width || targetFieldIndexX < 0 || targetFieldIndexY >= _size.Height || targetFieldIndexY < 0) return null;
            else return _fields[targetFieldIndexX][targetFieldIndexY].Character;
        }

        /// <summary>
        /// Resets the board: Clears all characters and items, but keeps the field attributes
        /// </summary>
        public void Clear()
        {
            foreach (List<Field> column in _fields) 
            {
                foreach (Field field in column)
                {
                    field.Item = null;
                    field.Character = null;
                }
            }
        }
    }
}
