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
        public static EnemyController CreateEnemy(Vector2 Position, bool right, GameObject EnemyObject)
        {
            float speedEnemy;
            int maxHealthEnemy;
            Vector2 scale = new Vector2(0.15f, 0.15f);
            Texture texture;
            Transform transformEnemy = new Transform(Position, 0, scale);
            Collider colliderEnemy = new Collider("enemy", EnemyObject);
            speedEnemy = 50f;
            maxHealthEnemy = 500;
            texture = Engine.GetTexture("Textures/NormalEnemy/Right/Idle Animation/Idle_1.png");
            Renderer renderEnemy = new Renderer(texture, transformEnemy);
            return new EnemyController(speedEnemy, maxHealthEnemy, right, transformEnemy, renderEnemy, colliderEnemy);

        }
        public static PatrolEnemy CreatePatrolEnemy(Vector2 PointA, Vector2 PointB, Vector2 Position, bool right, GameObject EnemyObject, Vector2 Patrolspeed)
        {
            Vector2 scale = new Vector2(0.15f, 0.15f);
            Texture texture;
            Transform transformEnemy = new Transform(Position, 0, scale);
            Collider colliderEnemy = new Collider("patrol", EnemyObject);
            texture = Engine.GetTexture("Textures/NormalEnemy/Right/Idle Animation/Idle_1.png");
            Renderer Patrolrender = new Renderer(texture, transformEnemy);
            return new PatrolEnemy(Patrolspeed, right, PointA, PointB, transformEnemy, Patrolrender, colliderEnemy);
        }

    }
}
