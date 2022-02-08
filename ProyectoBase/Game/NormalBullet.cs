using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class NormalBullet : GameObject, IBullet
    {
        private float speed;
        private int damage;
        protected float coolingTime;
        private float currentCoolingTime;
        public event System.Action<IBullet> OnDeactivate;
        private FactoryBullets.bullets type;
        private bool isFacingRight;
        public float Speed { get => speed; set => speed = value; }
        public int Damage { get => damage; set => damage = value; }
        public float CoolingTime { get => coolingTime; private set => coolingTime = value; }
        public float CurrentCoolingTime { get => currentCoolingTime; private set => currentCoolingTime = value; }
        public FactoryBullets.bullets Type { get => type; set => type = FactoryBullets.bullets.explosive; }
        public bool IsFacingRight { set => isFacingRight = value; }

        public NormalBullet(float speed, int damage, bool right,
            Transform transform, Renderer render, Collider collider) : base(transform, render, collider)
        {
            Speed = speed;
            Damage = damage;
            type = FactoryBullets.bullets.normal;
            IsFacingRight = right;
        }
        public NormalBullet() : base()
        {
            Collider = new Collider("bullet", this);
            Transform = new Transform();
            Transform.Scale = new Vector2(0.07f, 0.07f);
            Render = new Renderer(Engine.GetTexture("Textures/Levels/BulletLeft.png"), Transform);
            Type = FactoryBullets.bullets.normal;
            Speed = 300f;
            Damage = 50;
            IsFacingRight = true;
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
                   if (gameObjectColltion != null)
                    {
                    switch (gameObjectColltion.Collider.Layer)
                    {
                        case "player":
                            PlayerController player = (PlayerController)gameObjectColltion;
                            player.GetDamage(damage);
                            Deactivate();
                            break;
                        case "enemy":
                            EnemyController enemy = (EnemyController)gameObjectColltion;
                            enemy.GetDamage(damage);
                            Deactivate();
                            break;
                        case "patrol":
                            PatrolEnemy patrol = (PatrolEnemy)gameObjectColltion;
                            patrol.GetDamage(damage);
                            Deactivate();
                            break;
                        default:
                            Deactivate();
                            break;
                    }

                }
            }
        }
        public void Deactivate()
        {
            Transform.Position = new Vector2(900, 900);
            Level.poolOfNormalBullets.Recicle(this);
            OnDeactivate?.Invoke(this);
        }
        public void Instantiate(Vector2 Position)
        {
        }
        public void Activate(Vector2 Position, bool right)
        {
            Transform.Position = Position;
            Render.Texture = right ? Engine.GetTexture("Textures/Levels/BulletRight.png") 
               : Engine.GetTexture("Textures/Levels/BulletLeft.png");
            isFacingRight = right;
        }

    }
}
