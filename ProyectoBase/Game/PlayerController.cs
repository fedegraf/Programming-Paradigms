using System;
using System.Collections.Generic;
using System.Linq;


namespace Game
{

    public delegate void VoidDelegate();
    public delegate int IntDelegate(int life);
    class PlayerController : GameObject, IDamageable
    {
        private List<Animation> animations = new List<Animation>();
        private Animation currentAnimation;
        private static float ShootingCooldown, CoolingCurrentTime;
        private int currentHealth, maxHealth = 500;
        private Vector2 speed;
        private bool isFacingRight = true;
        private bool isExplosiveWeaponAvailable;
        public event System.Action<IDamageable> OnDestroy;
        private FactoryBullets.bullets currentBullet;
        private float fuel;
        private float fuelUsing = 4.0f;
        private float maxFuel = 350.0f;
        private float jetForce = -100.0f;
        private float refuelRate = 6.5f;
        private bool isFlying = false;
        private const float INPUT_DELAY_TIME = 0.2f;
        private float currentInputDelayTime;
        public VoidDelegate onDie;
        public IntDelegate onLifeChange;
        public VoidDelegate onWin;
        public VoidDelegate GetExplosiveWeapon;
        public VoidDelegate ChangeWeapon;
        public bool IsDestroy { get; private set; }
        public bool isDestroyed { get; set; }
        public Vector2 Speed { get => speed; set => speed = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public FactoryBullets.bullets CurrentBullet { get => currentBullet; set => currentBullet = value; }
        public bool IsFacingRight { get => isFacingRight; private set => isFacingRight = value; }
        public PlayerController(Vector2 speed, Vector2 position, float rotation, Vector2 scale, Texture texture)
            : base(position, rotation, scale, texture, "player")
        {
            CreateAnimations();
            Speed = speed;
            currentHealth = MaxHealth;
            currentAnimation = GetAnimation("idleRightAnimation");
            Render.Texture = currentAnimation.CurrentFrame;
            ShootingCooldown = 0.5f;
            CoolingCurrentTime = 0.6f;
            currentBullet = FactoryBullets.bullets.normal;
            isExplosiveWeaponAvailable = false;
            fuel = maxFuel;
            Engine.Debug(Collider.Layer);
            Collider.OnCollition += OnCollition;
        }

        private void CreateAnimations()
        {
            List<Texture> idleRightFrames = new List<Texture>();
            for (int i = 1; i <= 1; i++)
            {
                idleRightFrames.Add(Engine.GetTexture($"Textures/Character/Right/idle_{i}.png"));
            }
            Animation idleRightAnimation = new Animation("idleRightAnimation", idleRightFrames, 1f, true);
            animations.Add(idleRightAnimation);

            List<Texture> idleLeftFrames = new List<Texture>();
            for (int i = 1; i <= 1; i++)
            {
                idleLeftFrames.Add(Engine.GetTexture($"Textures/Character/Left/idle_{i}.png"));
            }
            Animation idleLeftAnimation = new Animation("idleLeftAnimation", idleLeftFrames, 1f, true);
            animations.Add(idleLeftAnimation);


            List<Texture> walkLeftframes = new List<Texture>();
            for (int i = 1; i <= 12; i++)
            {
                walkLeftframes.Add(Engine.GetTexture($"Textures/Character/Left/Run Animation/walk_{i}.png"));
            }
            Animation runLeftAnimation = new Animation("runLeftAnimation", walkLeftframes, 0.1f, true);
            animations.Add(runLeftAnimation);

            List<Texture> walkRightframes = new List<Texture>();
            for (int i = 1; i <= 12; i++)
            {
                walkRightframes.Add(Engine.GetTexture($"Textures/Character/Right/Run Animation/walk_{i}.png"));
            }
            Animation runRightAnimation = new Animation("runRightAnimation", walkRightframes, 0.1f, true);
            animations.Add(runRightAnimation);


            List<Texture> flyRightFrames = new List<Texture>();
            for (int i = 1; i <= 1; i++)
            {
                flyRightFrames.Add(Engine.GetTexture($"Textures/Character/Right/fly_{i}.png"));
            }
            Animation flyRightAnimation = new Animation("flyRightAnimation", flyRightFrames, 1f, true);
            animations.Add(flyRightAnimation);

            List<Texture> flyLeftFrames = new List<Texture>();
            for (int i = 1; i <= 1; i++)
            {
                flyLeftFrames.Add(Engine.GetTexture($"Textures/Character/Left/fly_{i}.png"));
            }
            Animation flyLeftAnimation = new Animation("flyLeftAnimation", flyLeftFrames, 1f, true);
            animations.Add(flyLeftAnimation);

        }

        public override void Update()
        {
            currentInputDelayTime += Time.deltaTime;
            CoolingCurrentTime += Time.DeltaTime;
            checkInput();
            currentAnimation.Update();
            Render.Texture = currentAnimation.CurrentFrame;
            if (!CheckCollisitionWithTiles())
            {
                ApplyGravity();
            }
            else if (!isFlying)
            {
                Speed = new Vector2(Speed.X, 0);
            }
            CheckColitions();
        }
        public void checkInput()
        {
            if (Engine.GetKey(Keys.D))
            {
                Transform.Position += new Vector2(Speed.X * Time.DeltaTime, 0);
                currentAnimation = GetAnimation("runRightAnimation");
                isFacingRight = true;
            }
            else if (Engine.GetKey(Keys.A))
            {
                Transform.Position -= new Vector2( Speed.X * Time.DeltaTime, 0);
                currentAnimation = GetAnimation("runLeftAnimation");
                isFacingRight = false;
            }
            else if (Engine.GetKey(Keys.S))
            {
                if (CoolingCurrentTime >= ShootingCooldown)
                {
                    Engine.Debug("Dispara");
                    Level.InstantiateBullet(Transform.Position, Render.Size, IsFacingRight, CurrentBullet.ToString(), "playerBullet");
                    CoolingCurrentTime = 0;
                }
            }
            else if (Engine.GetKey(Keys.W) && (fuel > 0))
            {
                currentAnimation = isFacingRight ? GetAnimation("flyRightAnimation") : GetAnimation("flyLeftAnimation");
                Speed = new Vector2(Speed.X, jetForce);
                Transform.Position += new Vector2(0, Speed.Y * Time.DeltaTime);
                isFlying = true;
                fuel -= fuelUsing;
                fuel = fuel < 0 ? 0 : fuel;
                Level.UserInterface.Fuelspent += fuelUsing;
                Level.UserInterface.Fuelspent = Level.UserInterface.Fuelspent > 350 ? 350 : Level.UserInterface.Fuelspent;
            }
            else
            {
                currentAnimation = isFacingRight ? GetAnimation("idleRightAnimation") : GetAnimation("idleLeftAnimation");
            }
            if (!Engine.GetKey(Keys.W))
            {
                isFlying = false;
                fuel += refuelRate;
                fuel = fuel > maxFuel ? maxFuel : fuel;
                Level.UserInterface.Fuelspent -= refuelRate;
                Level.UserInterface.Fuelspent = Level.UserInterface.Fuelspent < 0 ? 0 : Level.UserInterface.Fuelspent;
            }
            if (Engine.GetKey(Keys.I) && currentInputDelayTime >= INPUT_DELAY_TIME)
            {
                currentInputDelayTime = 0;
                if (isExplosiveWeaponAvailable)
                {
                    if (currentBullet == FactoryBullets.bullets.normal)
                    {
                        SetExplosiveWeapon();
                    }
                    else
                    {
                        SetNormalWeapon();
                    }
                }
            }
        }
        private void SetNormalWeapon()
        {
            CurrentBullet = FactoryBullets.bullets.normal;
            Level.UserInterface.IsNormalWeaponSet = true;
        }
        private void SetExplosiveWeapon()
        {
            CurrentBullet = FactoryBullets.bullets.explosive;
            Level.UserInterface.IsNormalWeaponSet = false;
            Engine.Debug("Set arma explosiva");
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
        public bool CheckCollisitionWithTiles()
        {
            bool IsColliding = false;
            for (int i = 0; i < Level.listOfTiles.Count; i++)
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
            public void ApplyGravity()
        {
            Speed += new Vector2 (0, Program.GRAVITY);
            Transform.Position += new Vector2(0, Speed.Y * Time.DeltaTime);
        }
        public void GetDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            Engine.Debug("Salud:" + currentHealth);
            Level.UserInterface.Lifebarlenght = currentHealth;
            Engine.Debug("Salud UI: " + Level.UserInterface.Lifebarlenght);
            onLifeChange?.Invoke(this.currentHealth);
            if (currentHealth < 0)
            {
                currentHealth = 0;
                Destroy();
            }
        }
        public void Destroy()
        {
            onDie?.Invoke();
            IsDestroy = true;
            isActive = false;
        }
        public void Healed()
        {
            currentHealth += 50;
            currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth;
            Level.UserInterface.Lifebarlenght = currentHealth;
            onLifeChange?.Invoke(this.currentHealth);
        }
        public void GetExplosiveAmmo()
        {
            isExplosiveWeaponAvailable = true;
            Level.UserInterface.IsExplosiveWeaponAvailable = true;
        }
        public void CheckColitions()
        {
            Collider.CheckCollitions();
        }
        public void OnCollition(GameObject gameobject)
        {
                    if (gameobject != null)
                    {
                        switch (gameobject.Collider.Layer)
                        {
                            case "AmmoPickup":
                                GetExplosiveAmmo();
                            gameobject.isActive = false;
                            gameobject.Transform.Position = new Vector2(-50, -50);
                                break;
                            case "Health":
                                Healed();
                                gameobject.isActive = false;
                                gameobject.Transform.Position = new Vector2(-50, -50);
                                break;
                            case "Win Trigger":
                                onWin?.Invoke();
                                break;
                        }
                    }
                }
            }
        }
