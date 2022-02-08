using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    class Explosion : GameObject
    {
        private int damage = 1;
        public Animation explosionAnimation;
        private float currentAnimationTime = 0;
        private int currentFrameIndex = 0;
        public Explosion(Vector2 Position) : base()
        {
            Engine.Debug("explosion");
            List<Texture> idleFrames = new List<Texture>();
            for (int i = 1; i <= 18; i++)
            {
                idleFrames.Add(Engine.GetTexture($"Textures/Levels/Explosion/{i}.png"));
            }
            explosionAnimation = new Animation("explosion", idleFrames, 0.1f, false);

            Collider = new Collider("explosion", this);
            Transform = new Transform();
            Transform.Position = Position;
            Transform.Scale = new Vector2(0.05f, 0.05f);
            Render = new Renderer(explosionAnimation.CurrentFrame, Transform);
        }
        public override void Update()
        {
            if (isActive)
            {
                explosionAnimation.Update();
                Render.Texture = explosionAnimation.CurrentFrame;

                currentAnimationTime += Time.DeltaTime;

                if (currentAnimationTime >= 0.1f)
                {
                    currentFrameIndex++;
                    currentAnimationTime = 0;
                    if (currentFrameIndex <= 9)
                    {
                        Transform.Scale += new Vector2(0.05f, 0.05f);
                        Render.Size = new Vector2(Render.RealWidht * Transform.Scale.X, Render.RealHeight * Transform.Scale.Y);
                        Render.OffSetX = Render.Size.X / 2;
                        Render.OffSetY = Render.OffSetY / 2;
                    }
                    else if (currentFrameIndex >= 18)
                    {
                        Transform.Position = new Vector2(900, 900);
                        isActive = false;
                    }
                }
                foreach (GameObject gameObject in Level.ActiveGameObjects.ToList())
                {
                    GameObject gameObjectColltion = Collider.CheckCollitions();
                    if (gameObjectColltion != null)
                    {
                        if (gameObjectColltion.Collider.Layer == "enemy" || gameObjectColltion.Collider.Layer == "player" || gameObjectColltion.Collider.Layer == "patrol")
                        {
                            IDamageable damageableObject = (IDamageable)gameObjectColltion;
                            damageableObject.GetDamage(damage);
                        }

                    }
                }
            }
        }

    }
}
