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
            Texture texture;
            Transform transformBullet = new Transform(Position, 0, scale);
            Collider colliderBullet = new Collider("bullet");
            switch (bullet)
            {
                case bullets.normal:
                    speed = 150f;
                    damage = 50;
                    texture = Engine.GetTexture("Textures/Levels/Bullet.png");
                    Renderer renderer = new Renderer(texture, transformBullet);
                    NormalBullet myBullet = new NormalBullet(speed, damage, true, transformBullet, renderer, colliderBullet);
                    myBullet.Collider.ThisGameObject = myBullet;
                    return myBullet;

                case bullets.explosive:
                    speed = 75f;
                    damage = 100;
                    texture = Engine.GetTexture("Textures/Levels/explosive_bullet.png");
                    Renderer explosive_renderer = new Renderer(texture, transformBullet);
                    ExplosiveBullet myExplosiveBullet =  new ExplosiveBullet(speed, damage, false, transformBullet, explosive_renderer, colliderBullet);
                    myExplosiveBullet.Collider.ThisGameObject = myExplosiveBullet;
                    return myExplosiveBullet;

                default:
                    speed = 150f;
                    damage = 50;
                    texture = Engine.GetTexture("Textures/Levels/Bullet.png");
                    Renderer default_renderer = new Renderer(texture, transformBullet);
                    NormalBullet myDefaultBullet = new NormalBullet(speed, damage, true, transformBullet, default_renderer, colliderBullet);
                    myDefaultBullet.Collider.ThisGameObject = myDefaultBullet;
                    return myDefaultBullet;
            }
        }

    }
}