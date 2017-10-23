using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TheQuest
{
    internal class Game
    {
        public const int INVENTORY_SIZE = 5;

        private TheQuestForm _form;
        private Level _currentLevel;
        private Player _player;
        private int _levelNumber = 1; //1-indicated

        public Game(TheQuestForm form)
        {
            _form = form;
            _player = new Player(INVENTORY_SIZE);
            _player.PickUpItem(new Sword(), out _);
        }

        public Level CurrentLevel
        {
            get { return _currentLevel; }
        }
        public Player Player
        {
            get { return _player; }
        }

        /// <summary>
        /// Starts the next level and builds up the room
        /// </summary>
        public void NextLevel()
        {
            _currentLevel = new Level(_levelNumber);
            _form.BuildRoom(new Point(0, 0));
            Spawn(_player);
            foreach(Item item in _currentLevel.Items)
            {
                Spawn(item);
            }
            PlayRound();
        }

        private void PlayRound()
        {
            if (CheckGameStatus() == GameStatus.LevelComplete)
            {
                EndLevel();
                return;
            }
            foreach (Enemy enemy in CurrentLevel.Enemies)
            {
                if (enemy.IsAlive)
                {
                    EnemyTurn(enemy);
                    if (CheckGameStatus() == GameStatus.GameOver)
                    {
                        EndGame();
                        return;
                    }
                }
            }
            PlayerTurn();
        }

        private void PlayerTurn()
        {
            _form.EnableControls();
        }

        private void EnemyTurn(Enemy currentEnemy)
        {
            _form.DisableControls();
            if (!currentEnemy.IsSpawned)
            {
                TrySpawn(currentEnemy, out bool success);
                if (!success) return;
            }

            for (int i = 0; i < currentEnemy.MovementSpeed; i++)
            {
                List<Direction> possibleDirections;
                int distanceToPlayer = CurrentLevel.Board.GetDistanceBetween(_player, currentEnemy);
                switch (currentEnemy.GetActionType(distanceToPlayer))
                {
                    case EnemyActionType.MoveRandom:
                        Console.WriteLine($"{currentEnemy.GetType().ToString()} moves randomly.");
                        Random random = new Random();
                        possibleDirections = new List<Direction>();
                        possibleDirections.AddRange((IEnumerable<Direction>)Enum.GetValues(typeof(Direction)));
                        for (int j = 3; j >= 0; j--)
                        {
                            Direction direction = possibleDirections[random.Next(j)];
                            try
                            {
                                Move(currentEnemy, direction);
                                break;
                            }
                            catch (BoardException be)
                            {
                                possibleDirections.Remove(direction);
                            }
                        }
                        break;

                    case EnemyActionType.MoveToPlayer:
                        Console.WriteLine($"{currentEnemy.GetType().ToString()} moves towards the player.");
                        try
                        {
                            Move(currentEnemy, CurrentLevel.Board.GetDirectionTowards(currentEnemy, _player));
                        }
                        catch (BoardException)
                        {

                        }
                        break;

                    case EnemyActionType.Attack:
                        Console.WriteLine($"{currentEnemy.GetType().ToString()} attacks.");
                        Rotate(currentEnemy, CurrentLevel.Board.GetDirectionTowards(currentEnemy, _player));

                        foreach(Target currentTarget in currentEnemy.Attack())
                        {
                            Character targetCharacter = CurrentLevel.Board.GetTargetCharacter(currentEnemy, currentTarget);
                            if (targetCharacter != null)
                            {
                                targetCharacter.TakeDamage(currentEnemy.AttackStrength);
                                UpdateBoard();
                            }
                        }

                        return; //Only attack once, regardless of movement speed

                    case EnemyActionType.DoNothing:
                        Console.WriteLine($"{currentEnemy.GetType().ToString()} does nothing.");
                        break;
                }
            }
        }

        private GameStatus CheckGameStatus()
        {
            if (!_player.IsAlive)
            {
                return GameStatus.GameOver;
            }
            else
            {
                bool AreEnemiesAlive = false;
                foreach(Enemy enemy in _currentLevel.Enemies)
                {
                    if (enemy.IsAlive)
                    {
                        AreEnemiesAlive = true;
                        break;
                    }
                }

                if (!AreEnemiesAlive)
                {
                    return GameStatus.LevelComplete;
                }
                else
                {
                    return GameStatus.Running;
                }
            }
        }

        private void Spawn(IPlaceable objectToSpawn)
        {
            //Find matching spawnpoints
            List<Field> spawnPoints = CurrentLevel.Board.GetSpawnPoints(objectToSpawn);
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                //Make sure that no fields which are already occupied are considered
                if ((objectToSpawn is Character && spawnPoints[i].Character != null)
                 || (objectToSpawn is Item && spawnPoints[i].Item != null))
                {
                    spawnPoints.RemoveAt(i);
                    i--;
                }
            }

            if (spawnPoints.Count == 0) return;

            Random random = new Random();
            Field spawnPoint = spawnPoints[random.Next(spawnPoints.Count)];
            objectToSpawn.Field = spawnPoint;
            objectToSpawn.IsSpawned = true;
            
            UpdateBoard();
        }

        private void TrySpawn(Character character, out bool success)
        {
            Spawn(character);
            success = character.IsSpawned;
        }

        private void UpdateBoard()
        {
            CurrentLevel.Board.Clear();
            if (_player.Field != null && _player.IsAlive)
            {
                _player.Field.Character = Player;
            }
            foreach(Enemy enemy in CurrentLevel.Enemies)
            {
                if (enemy.Field != null && enemy.IsAlive)
                {
                    enemy.Field.Character = enemy;
                }
            }
            foreach (Item item in CurrentLevel.Items)
            {
                if (item.Field != null && item.IsSpawned)
                {
                    item.Field.Item = item;
                }
            }
            _form.UpdateSprites();
        }

        /// <summary>
        /// Moves an object one field on the board.
        /// If it is a character, also rotate the object so that it's facing the direction it walks in.
        /// If it is a player, end his round and play next one.
        /// </summary>
        /// <param name="thingToMove">the object to move</param>
        /// <param name="direction">the direction to move the object along</param>
        public void Move(IMoveable thingToMove, Direction direction)
        {
            CurrentLevel.Board.Move(thingToMove, direction);

            if (thingToMove is Character)
            {
                Rotate(thingToMove as Character, direction);
            }
            UpdateBoard();

            if (thingToMove is Player)
            {
                PlayRound();
            }
        }

        /// <summary>
        /// Rotates a character
        /// </summary>
        /// <param name="thingToRotate">character to rotate</param>
        /// <param name="direction">direction to set</param>
        public void Rotate(Character thingToRotate, Direction direction)
        {
            thingToRotate.Orientation = direction;
            UpdateBoard();
        }

        /// <summary>
        /// Makes the current player use the item of the specified slot. Attacks if item is weapon, uses it up, if it is a potion.
        /// </summary>
        /// <param name="slotNumber">The slot number of the player's inventory to pick the item from.</param>
        public void UseItem(int slotNumber)
        {
            Item item = _player.Inventory.Slots[slotNumber].Item;
            if (item is Weapon)
            {
                foreach (Target currentTarget in _player.Attack((Weapon)item))
                {
                    Character targetCharacter = CurrentLevel.Board.GetTargetCharacter(_player, currentTarget);
                    if (targetCharacter != null)
                    {
                        int damage = ((Weapon)item).DamagePoints;
                        if (item is Bow && _player.Field.Attribute == FieldAttribute.FireBonus)
                        {
                            damage += ((Bow)item).FireBonusDamage;
                        }
                        targetCharacter.TakeDamage(damage);
                        UpdateBoard();
                    }
                }
            }
            else if (item is Potion)
            {
                _player.ConsumePotion((Potion)item);
            }
            UpdateBoard();
            PlayRound();
        }

        private void EndLevel()
        {
            _levelNumber++;
            MessageBox.Show($"Level {_levelNumber - 1} completed!\r\nStarting Level {_levelNumber} now...");
            NextLevel();
        }

        private void EndGame()
        {
            _form.DisableControls();
            MessageBox.Show("GAME OVER!");
        }

    }
}
