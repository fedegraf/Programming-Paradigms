using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class ExplosiveBullet : GameObject, IBullet
    {
        private float speed;
        private int damage;
        protected float coolingTime;
        private float currentCoolingTime;
        public event System.Action<IBullet> OnDeactivate;
        private FactoryBullets.bullets type;
        private bool isFacingRight;
        private bool AlreadyDone = false;
        Explosion explosion;

        public float Speed { get => speed; set => speed = value; }
        public int Damage { get => damage; set => damage = value; }
        public float CoolingTime { get => coolingTime; private set => coolingTime = value; }
        public float CurrentCoolingTime { get => currentCoolingTime; private set => currentCoolingTime = value; }
        public FactoryBullets.bullets Type { get => type; set => type = FactoryBullets.bullets.explosive; }
        public bool IsFacingRight { set => isFacingRight = value; }
        public ExplosiveBullet(float speed, int damage, bool Right, Vector2 position, float rotation, Vector2 scale)
            : base(position, rotation, scale, Engine.GetTexture("Textures/Levels/explosive_bullet.png"), "playerBullet")
        {
            Speed = speed;
            Damage = damage;
            type = FactoryBullets.bullets.explosive;
            IsFacingRight = Right;
            Collider.OnCollition += OnCollition;
        }
        public ExplosiveBullet() : base()
        {
            type = FactoryBullets.bullets.explosive;
            Transform.Scale = new Vector2 (0.1f, 0.1f);
            Collider.OnCollition += OnCollition;
        }

        public override void Update()
        {
            if (isFacingRight)
            {
                Transform.Position += new Vector2(1, 0) * speed * Time.DeltaTime;
            }
            else
            {
                Transform.Position -= new Vector2(1, 0) * speed * Time.DeltaTime;
            }
            //Deactivate Bullet if is out of screen
            if (Transform.Position.X < 0 || Transform.Position.X > Program.SCREEN_WIDHT)
            {
                Deactivate();
            }
            {
                Collider.CheckCollitions();
            }

        }

        public void OnCollition(GameObject gameObjectCollition)
        {
            if (!AlreadyDone)
            {
                switch (gameObjectCollition.Collider.Layer)
                {
                    //If the bullet collide with anything but the one who trigger it, create an explotion
                    case "player":
                        if (Collider.Layer != "playerBullet")
                        {
                            Engine.Debug("Case Player");
                            CreateExplotion();
                        }
                        break;
                    case "enemy":
                        if (Collider.Layer != "EnemyBullet")
                        {
                            CreateExplotion();
                        }
                        break;
                    case "patrol":
                        if (Collider.Layer != "EnemyBullet")
                        {
                            CreateExplotion();
                        }
                        break;
                    default:
                        break;
                }
            }

        }
        public void Deactivate()
        {
            Transform.Position = new Vector2(900, 900);
            Level.poolOfExplosiveBullets.Recicle(this);
            OnDeactivate?.Invoke(this);
        }
        public void Instantiate(Vector2 Position)
        {        }
        public void Activate(Vector2 Position, bool right, String Layer)
        {
            Transform.Position = Position;
            Render.Texture = right ? Engine.GetTexture("Textures/Levels/ExplosiveBulletRight.png") : 
                Engine.GetTexture("Textures/Levels/ExplosiveBulletLeft.png");
            isFacingRight = right;
            Collider.Layer = Layer;
            Render.RealHeight = Render.Texture.Height;
            Render.RealWidht =  Render.Texture.Width;
            Render.Size = new Vector2(Render.RealWidht * Transform.Scale.X, Render.RealHeight * Transform.Scale.Y);
            Render.OffSetX = Render.Size.X / 2;
            Render.OffSetY = Render.Size.Y / 2;
            speed = 100f;
        }
        public void CreateExplotion()
        {
            AlreadyDone = false;
            explosion = new Explosion(this.Transform.Position);
            Deactivate();
            Level.ActiveGameObjects.Add(explosion);
        }
    }
}
