using System.Collections.Generic;

namespace Game
{
    public static class FactoryBullets
    {
        public enum bullets
        {
            normal,
            explosive
        }
        public static IBullet CreateBullet(bullets bullet, Vector2 Position)
        {
            float speed;
            int damage;
            Vector2 scale = new Vector2(0.15f, 0.15f);
            switch (bullet)
            {
                case bullets.normal:
                    speed = 150f;
                    damage = 50;
                    NormalBullet myBullet = new NormalBullet(speed, damage, true, Position, 0, scale);
                    myBullet.Collider.ThisGameObject = myBullet;
                    return myBullet;

                case bullets.explosive:
                    speed = 75f;
                    damage = 100;
                    ExplosiveBullet myExplosiveBullet =  new ExplosiveBullet(speed, damage, false, Position, 0, scale);
                    myExplosiveBullet.Collider.ThisGameObject = myExplosiveBullet;
                    return myExplosiveBullet;

                default:
                    speed = 150f;
                    damage = 50;
                    NormalBullet myDefaultBullet = new NormalBullet(speed, damage, true, Position, 0, scale);
                    myDefaultBullet.Collider.ThisGameObject = myDefaultBullet;
                    return myDefaultBullet;
            }
        }

    }
}