using System;
using System.Collections.Generic;

namespace Game
{
    public class EnemyController : GameObject, IDamageable, IEnemy
    {
        private List<Animation> animations = new List<Animation>();
        private Animation currentAnimation;
        private static float ShootingCooldown, CoolingCurrentTime;
        private int currentHealth, maxHealth = 250;
        private Vector2 speed;
        private bool isFacingRight;
        private float ShootAnimationLenght = 0.8f;
        private float CurrentShootAnimationTime;

        public Vector2 Speed { get => speed; set => speed = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public bool IsFacingRight { get => isFacingRight; set => isFacingRight = value; }
        public bool isDestroyed => throw new NotImplementedException();

        public EnemyController(float speed, int maxHealth, bool right, Vector2 position, float rotation, Vector2 scale)
        : base(position, rotation, scale, Engine.GetTexture("Textures/NormalEnemy/Right/Idle Animation/Idle_1.png"), "enemy")
        {
            CreateNormalEnemyAnimation();
            Engine.Debug("Creo animaciones");
            Speed = new Vector2 (speed,0);
            MaxHealth = maxHealth;
            currentHealth = MaxHealth;
            IsFacingRight = right;
            currentAnimation = right ? GetAnimation("idleRightAnimation") : GetAnimation("idleLeftAnimation");
            Engine.Debug("Seteo animacion correcta");
            ShootingCooldown = 4.5f;
            CoolingCurrentTime = 0f;
        }

        event Action<IDamageable> IDamageable.OnDestroy
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public override void Update()
        {
            CoolingCurrentTime += Time.DeltaTime;
            CurrentShootAnimationTime += Time.DeltaTime;
            currentAnimation.Update();
            Render.Texture = currentAnimation.CurrentFrame;
            if (CurrentShootAnimationTime >= ShootAnimationLenght)
            {
                currentAnimation = isFacingRight ? GetAnimation("idleRightAnimation") : GetAnimation("idleLeftAnimation");
            }
        }
        public Animation GetAnimation(string Id)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                if (animations[i].Id == Id)
                {
                    return animations[i];
                }
            }
            Engine.Debug($"Animation not found: {Id}");
            return null;
        }
        public void Shoot()
        {
            if (CoolingCurrentTime >= ShootingCooldown)
            {
                CurrentShootAnimationTime = 0;
                currentAnimation = isFacingRight ? GetAnimation("ShootRightAnimation") : GetAnimation("ShootLeftAnimation");
                currentAnimation.Play();
                Render.Size = new Vector2(currentAnimation.CurrentFrame.Width * Transform.Scale.X, Render.Size.Y);
                Level.InstantiateBullet(Transform.Position, Render.Size, isFacingRight, "normal", "EnemyBullet");
                CoolingCurrentTime = 0;
            }
        }
        public void GetDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Destroy();
            }
        }
        public void Destroy()
        {
            isActive = false;
            Transform.Position = new Vector2 (1800, 1000);
        }
        private List<Animation> CreateNormalEnemyAnimation()
        {
            List<Texture> idleRightFrames = new List<Texture>();
            for (int i = 1; i <= 10; i++)
            {
                idleRightFrames.Add(Engine.GetTexture($"Textures/NormalEnemy/Right/Idle Animation/Idle_{i}.png"));
            }
            Animation idleRightAnimation = new Animation("idleRightAnimation", idleRightFrames, 0.1f, true);
            animations.Add(idleRightAnimation);

            List<Texture> idleLeftFrames = new List<Texture>();
            for (int i = 1; i <= 10; i++)
            {
                idleLeftFrames.Add(Engine.GetTexture($"Textures/NormalEnemy/Left/Idle Animation/Idle_{i}.png"));
            }
            Animation idleLeftAnimation = new Animation("idleLeftAnimation", idleLeftFrames, 0.1f, true);
            animations.Add(idleLeftAnimation);

            List<Texture> shootRightFrames = new List<Texture>();
            for (int i = 1; i <= 4; i++)
            {
                shootRightFrames.Add(Engine.GetTexture($"Textures/NormalEnemy/Right/Shoot Animation/Shoot_{i}.png"));
            }
            Animation shootRightAnimation = new Animation("ShootRightAnimation", shootRightFrames, 0.2f, false);
            animations.Add(shootRightAnimation);

            List<Texture> shootLeftFrames = new List<Texture>();
            for (int i = 1; i <= 4; i++)
            {
                shootLeftFrames.Add(Engine.GetTexture($"Textures/NormalEnemy/Left/Shoot Animation/Shoot_{i}.png"));
            }
            Animation shootLeftAnimation = new Animation("ShootLeftAnimation", shootLeftFrames, 0.2f, false);
            animations.Add(shootLeftAnimation);

            return animations;
        }
    }
}
