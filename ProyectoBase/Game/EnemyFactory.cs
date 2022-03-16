using System.Collections.Generic;

namespace Game
{
    public static class EnemyFactory
    {
        public enum Enemy
        {
            normal,
            patrol
        }
        public static IEnemy CreateEnemy(Enemy enemyType, Vector2 Position, bool right, GameObject EnemyObject, Vector2 PointA, Vector2 PointB, Vector2 Patrolspeed)
        {
            IEnemy enemy;
            float speedEnemy;
            int maxHealthEnemy;
            Vector2 scale = new Vector2(0.15f, 0.15f);
            Transform transformEnemy = new Transform(Position, 0, scale);

            speedEnemy = 50f;
            maxHealthEnemy = 500;
            if (enemyType == Enemy.normal)
            {
              enemy = (IEnemy)new EnemyController(speedEnemy, maxHealthEnemy, right, Position, 0, scale);
            }
            else
            {
                enemy = (IEnemy) new PatrolEnemy(Patrolspeed, right, PointA, PointB, Position, 0, scale);
            }
            return enemy;
        }

    }
}
