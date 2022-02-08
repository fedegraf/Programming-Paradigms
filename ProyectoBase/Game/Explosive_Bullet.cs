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

        public float Speed { get => speed; set => speed = value; }
        public int Damage { get => damage; set => damage = value; }
        public float CoolingTime { get => coolingTime; private set => coolingTime = value; }
        public float CurrentCoolingTime { get => currentCoolingTime; private set => currentCoolingTime = value; }
        public FactoryBullets.bullets Type { get => type; set => type = FactoryBullets.bullets.explosive; }
        public bool IsFacingRight { set => isFacingRight = value; }
        public ExplosiveBullet(float speed, int damage, bool Right,
            Transform transform, Renderer render, Collider collider) : base(transform, render, collider)
        {
            Speed = speed;
            Damage = damage;
            type = FactoryBullets.bullets.explosive;
            IsFacingRight = Right;
            collider.ThisGameObject = this;
        }
        public ExplosiveBullet() : base()
        {
            Collider = new Collider("bullet", this);
            Transform = new Transform();
            Transform.Scale = new Vector2(0.1f, 0.1f);
            Render = new Renderer(Engine.GetTexture("Textures/Levels/ExplosiveBulletLeft.png"), Transform);
            Type = FactoryBullets.bullets.explosive;
            Speed = 200f;
            Damage = 50;

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

            if (Transform.Position.X < 0 || Transform.Position.X > Program.SCREEN_WIDHT)
            {
                Deactivate();
            }
            foreach (GameObject gameObject in Level.ActiveGameObjects.ToList())
            {
                GameObject gameObjectColltion = Collider.CheckCollitions();
                if (gameObjectColltion != null && !AlreadyDone)
                {
                    AlreadyDone = false;
                    Explosion explosion = new Explosion(this.Transform.Position);
                    Deactivate();
                    Level.ActiveGameObjects.Add(explosion);
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
        public void Activate(Vector2 Position, bool right)
        {
            Engine.Debug("Bala activada");
            Transform.Position = Position;
            Render.Texture = right ? Engine.GetTexture("Textures/Levels/ExplosiveBulletRight.png") : 
                Engine.GetTexture("Textures/Levels/ExplosiveBulletLeft.png");
            isFacingRight = right;
        }
    }
}
