using System;
using System.Collections.Generic;

namespace Game
{
    public class EnemyController : GameObject, IDamageable
    {
        private List<Animation> animations = new List<Animation>();
        private Animation currentAnimation;
        private static float ShootingCooldown, CoolingCurrentTime;
        private int currentHealth, maxHealth;
        private float speed;
        private bool isFacingRight;
        private float ShootAnimationLenght = 0.8f;
        private float CurrentShootAnimationTime;

        public float Speed { get => speed; set => speed = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public bool IsFacingRight { get => isFacingRight; set => isFacingRight = value; }
        public bool isDestroyed => throw new NotImplementedException();

        public EnemyController(float speed, int maxHealth, bool right,
            Transform transform, Renderer render, Collider collider) : base(transform, render, collider)
        {
            CreateNormalEnemyAnimation();
            Engine.Debug("Creo animaciones");
            Speed = speed;
            MaxHealth = maxHealth;
            currentHealth = MaxHealth;
            IsFacingRight = right;
            currentAnimation = right ? GetAnimation("idleRightAnimation") : GetAnimation("idleLeftAnimation");
            Engine.Debug("Seteo animacion correcta");
            ShootingCooldown = 4.5f;
            CoolingCurrentTime = 0f;
            collider.ThisGameObject = this;
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
            Engine.Debug($"No se encontro la animacion: {Id}");
            return null;
        }
        public bool CheckCollisitionWithTiles(List<Tile> Tiles)
        {
            bool IsColliding = false;
            for (int i = 0; i < Tiles.Count; i++)
            {
                IsColliding = CollisionsUtilities.isBoxingColliding(Transform.Position, Render.Size, Level.listOfTiles[i].Transform.Position,
                    Level.listOfTiles[i].Render.Size);
                if (IsColliding)
                {
                    return IsColliding;
                }
            }
            return IsColliding;
        }
        public void Shoot()
        {
            if (CoolingCurrentTime >= ShootingCooldown)
            {
                CurrentShootAnimationTime = 0;
                currentAnimation = isFacingRight ? GetAnimation("ShootRightAnimation") : GetAnimation("ShootLeftAnimation");
                currentAnimation.Play();
                Render.Size = new Vector2(currentAnimation.CurrentFrame.Width * Transform.Scale.X, Render.Size.Y);
                Level.InstantiateBullet(Transform.Position, Render.Size, isFacingRight, "normal");
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
        public void OnDestroy()
        {
        }

        public void Destroy()
        {
            isActive = false;
            Transform.Position = new Vector2 (1800, 1000);
         //   Level.ActiveGameObjects.Remove(this);
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


            /*            List<Texture> walkframes = new List<Texture>();
                        for (int i = 1; i <= 12; i++)
                        {
                            walkframes.Add(Engine.GetTexture($"Textures/Character/Run Animation/walk_{i}.png"));
                        }
                        Animation runAnimation = new Animation("runAnimation", walkframes, 0.1f, true);
                        animations.Add(runAnimation);

            */
            return animations;
        }
    }
}
