using System.Collections.Generic;

namespace TheQuest
{

    abstract internal class Enemy : Character
    {
        private int _attackStrength;
        private int _movementSpeed; //How many fields the enemy can walk per round
        private int _trackingDistance; //Over what distance the enemy can track a player and move towards him

        public Enemy(int hitPoints, int attackStrength, int movementSpeed, int trackingDistance) : base(hitPoints)
        {
            _attackStrength = attackStrength;
            _movementSpeed = movementSpeed;
            _trackingDistance = trackingDistance;
        }

        public int AttackStrength
        {
            get { return _attackStrength; }
        }

        public int MovementSpeed
        {
            get { return _movementSpeed; }
        }

        /// <summary>
        /// Returns a list of Target objects which the enemy can currently attack.
        /// </summary>
        /// <returns>List of targets representing the relative distance to the target from the enemy</returns>
        public virtual List<Target> Attack()
        {
            return new List<Target>() { new Target(1, 0, 0.5D) };
        }

        /// <summary>
        /// Returns what action the enemy will do. 
        /// - Attacks if enemy stands right next to player
        /// - Moves towards player if player is within tracking distance
        /// - Otherwise moves randomly
        /// </summary>
        /// <param name="distanceToPlayer">How far away the closest player is.</param>
        /// <returns>An EnemyActionType representing the enemies next action</returns>
        public virtual EnemyActionType GetActionType(int distanceToPlayer)
        {
            if (distanceToPlayer == 1) return EnemyActionType.Attack;
            else if (distanceToPlayer <= _trackingDistance || _trackingDistance == -1) return EnemyActionType.MoveToPlayer;
            else return EnemyActionType.MoveRandom;
        }
    }
}
