using System;

namespace TheQuest
{
    internal class Ghost : Enemy
    {
        public Ghost() : base(4, 3, 1, -1) { }

        /// <summary>
        /// Returns what action the ghost will do. 
        /// - Attacks if enemy stands right next to player
        /// - Chance that the ghost does nothing or moves towards the player.
        /// </summary>
        /// <param name="distanceToPlayer">How far away the closest player is.</param>
        /// <returns>An EnemyActionType representing the ghost's next action</returns>
        public override EnemyActionType GetActionType(int distanceToPlayer)
        {
            Random random = new Random();
            if (random.Next(3) == 0 || distanceToPlayer == 1)
            {
                return base.GetActionType(distanceToPlayer);
            }
            else
            {
                return EnemyActionType.DoNothing;
            }
        }
    }
}
