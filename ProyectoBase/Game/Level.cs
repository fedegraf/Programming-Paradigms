using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Level : IScene
    {
        private string currentLevel;
        private string backgroundTexturePath;
        private static PlayerController player;
        public static EnemyController enemy;
        public static PatrolEnemy patrolenemy;
        public static List<Tile> listOfTiles = new List<Tile>();
        public static Pool<ExplosiveBullet> poolOfExplosiveBullets = new Pool<ExplosiveBullet>();
        public static Pool<NormalBullet> poolOfNormalBullets = new Pool<NormalBullet>();
        public static List<GameObject> ActiveGameObjects = new List<GameObject>();
        public static List<GameObject> NormalEnemies = new List<GameObject>();
        public static List<GameObject> PatrolEnemies = new List<GameObject>();
        public static Interface UserInterface;
        public string BackgroundTexturePath { get => backgroundTexturePath; set => backgroundTexturePath = value; }
        public GameManager.GameState Id => GameManager.GameState.Level;
        public void Initialize()
        {
            //Create Player
            Vector2 speedPlayer = new Vector2(100f, 0f);
            Vector2 PlayerPosition = new Vector2(50, 50);
            Vector2 PlayerScale = new Vector2(0.15f, 0.15f);
            player = new PlayerController(speedPlayer, PlayerPosition, 0, PlayerScale, Engine.GetTexture("Textures/Character/Left/idle_1.png"));
            //Create UI
            UserInterface = new Interface();
            //Load first level
            LoadLevel1();
            player.onDie += GameOverHandler;
            player.onWin += WinHandler;
        }
        public void Update()
        {
            //Update every active Game Object
            foreach (GameObject gameObject in ActiveGameObjects.ToList())
            {
                if (gameObject.isActive)
                {
                    gameObject.Update();
                }
            }
            if (poolOfNormalBullets.Used.Count != 0)
            {
                foreach (NormalBullet bullet in poolOfNormalBullets.Used.ToList())
                {
                    bullet.Update();
                }

            }
            if (poolOfExplosiveBullets.Used.Count != 0)
            {
                foreach (ExplosiveBullet bullet in poolOfExplosiveBullets.Used.ToList())
                {
                    bullet.Update();
                }
            }
            UpdateEnemies();
            UserInterface.Update();
        }
        public void Render()
        {
            Engine.Draw(backgroundTexturePath, 0, 0, 0.75f, 0.75f);
            DrawTiles();
            foreach (GameObject gameObject in ActiveGameObjects)
            {
                if (gameObject.isActive)
                {
                    gameObject.Render.Render(gameObject.Transform);
                }
            }
            foreach (NormalBullet bullet in poolOfNormalBullets.Used)
            {
                GameObject gameObject = (GameObject)bullet;
                gameObject.Render.Render(gameObject.Transform);
            }

            foreach (ExplosiveBullet bullet in poolOfExplosiveBullets.Used)
            {
                GameObject gameObject = (GameObject)bullet;
                gameObject.Render.Render(gameObject.Transform);
            }
            UserInterface.Render();
        }
        private static void DrawTiles()
        {
            listOfTiles.ForEach(tile =>
            Engine.Draw(tile.Render.Texture, tile.Transform.Position.X, tile.Transform.Position.Y,
            tile.Transform.Scale.X, tile.Transform.Scale.Y, 0, tile.Render.OffSetX, tile.Render.OffSetY)
            );
        }
        private static void CreateTiles(int cantTiles, float PositionY, float positionXStart)
        {
            float PositionX = positionXStart;
            for (int i = 0; i <= cantTiles; i++)
            {
                Tile tile = new Tile();
                listOfTiles.Add(tile);
                tile.Transform.Position = new Vector2(PositionX, PositionY);
                PositionX += tile.Render.Size.X;
            }
        }
        public static void InstantiateBullet(Vector2 position, Vector2 size, bool right, String type, String Layer)
        {
            if (type == FactoryBullets.bullets.normal.ToString())
            {
                float spawnWidth = right ? (position.X + size.X / 2 + 15f) : (position.X - size.X / 2 - 15f);
                NormalBullet MyBullet = poolOfNormalBullets.Get();
                MyBullet.Activate(new Vector2(spawnWidth, position.Y), right, Layer);
            }
            else if (type == FactoryBullets.bullets.explosive.ToString())
            {
                float spawnWidth = right ? (position.X + size.X / 2 + 7.5f) : (position.X - size.X / 2 - 7.5f);
                ExplosiveBullet MyBullet = poolOfExplosiveBullets.Get();
                MyBullet.Activate(new Vector2(spawnWidth, position.Y), right, Layer);
                Engine.Debug("Disparo explosivo");
            }
        }
        public void WinHandler()
        {
            //See what level is loaded and load next one or the win screen if last
            switch (currentLevel)
            {
                case "Level 1":
                    LoadLevel2();
                    break;
                case "Level 2":
                    LoadLevel3();
                    break;
                case "Level 3":
                    GameManager.Instance.ChangeGameState(GameManager.GameState.WinScreen);
                    break;
                default:
                    GameManager.Instance.ChangeGameState(GameManager.GameState.WinScreen);
                    break;
            }
        }
        public static void GameOverHandler()
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.GameOverScreen);
        }
        private void LoadLevel1()
        {
            listOfTiles.Clear();
            ActiveGameObjects.Clear();
            CreateTiles(3, 100, 0);
            CreateTiles(4, 350, 250);
            CreateTiles(2, 450, 100);
            CreateTiles(4, 500, 500);
            enemy = (EnemyController)EnemyFactory.CreateEnemy(EnemyFactory.Enemy.normal, new Vector2(700, 300), false, enemy, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
            ActiveGameObjects.Add(enemy);
            NormalEnemies.Add(enemy);
            EnemyController enemy2 = (EnemyController)EnemyFactory.CreateEnemy(EnemyFactory.Enemy.normal, new Vector2(100, 400), true, enemy, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
            ActiveGameObjects.Add(enemy2);
            NormalEnemies.Add(enemy2);
            HealthItem healthitem = new HealthItem();
            healthitem.Transform.Position = new Vector2(700, 480);
            ActiveGameObjects.Add(healthitem);
            AmmoPickup ammopickup = new AmmoPickup();
            ammopickup.Transform.Position = new Vector2(350, 325);
            ActiveGameObjects.Add(ammopickup);
            WinTrigger winTrigger = new WinTrigger();
            winTrigger.Transform.Position = new Vector2(950, 440);
            ActiveGameObjects.Add(winTrigger);
            BackgroundTexturePath = "Textures/Levels/Background.png";
            currentLevel = "Level 1";
            ActiveGameObjects.Add(player);
            player.Transform.Position = new Vector2(50, 25);

        }
        private void LoadLevel2()
        {
            ActiveGameObjects.Clear();
            listOfTiles.Clear();
            BackgroundTexturePath = "Textures/Levels/Background_Level_2.png";
            currentLevel = "Level 2";
            CreateTiles(5, 350, 500);
            CreateTiles(5, 500, 50);
            CreateTiles(7, 220, 50);
            CreateTiles(6, 120, 00);
            patrolenemy = (PatrolEnemy) EnemyFactory.CreateEnemy(EnemyFactory.Enemy.patrol, new Vector2(100, 80), true, patrolenemy, new Vector2(150, 80), new Vector2(600, 80), new Vector2(50, 0));
            ActiveGameObjects.Add(patrolenemy);
            PatrolEnemies.Add(patrolenemy);
            patrolenemy = (PatrolEnemy) EnemyFactory.CreateEnemy(EnemyFactory.Enemy.patrol, new Vector2(50, 180), false, patrolenemy, new Vector2(500, 180), new Vector2(800, 180), new Vector2(150, 0));
            ActiveGameObjects.Add(patrolenemy);
            PatrolEnemies.Add(patrolenemy);
            patrolenemy = (PatrolEnemy)EnemyFactory.CreateEnemy(EnemyFactory.Enemy.patrol, new Vector2(500, 300), true, patrolenemy, new Vector2(500, 300), new Vector2(900, 300), new Vector2(100, 0));
            ActiveGameObjects.Add(patrolenemy);
            PatrolEnemies.Add(patrolenemy);
            WinTrigger winTrigger = new WinTrigger();
            winTrigger.Transform.Position = new Vector2(40, 60);
            ActiveGameObjects.Add(winTrigger);
            ActiveGameObjects.Add(player);
            player.Transform.Position = new Vector2(25, 430);
        }
        private void LoadLevel3()
        {
            ActiveGameObjects.Clear();
            listOfTiles.Clear();
            PatrolEnemies.Clear();
            NormalEnemies.Clear();
            BackgroundTexturePath = "Textures/Levels/Background_Level_3.png";
            currentLevel = "Level 3";
            ActiveGameObjects.Add(player);
            player.Transform.Position = new Vector2(50, 25);
            CreateTiles(5, 350, 500);
            CreateTiles(5, 500, 50);
            CreateTiles(7, 220, 50);
            CreateTiles(3, 100, 0);
            CreateTiles(4, 350, 250);
            CreateTiles(4, 500, 500);
            HealthItem healthitem = new HealthItem();
            healthitem.Transform.Position = new Vector2(150, 70);
            ActiveGameObjects.Add(healthitem);
            WinTrigger winTrigger = new WinTrigger();
            winTrigger.Transform.Position = new Vector2(950, 440);
            ActiveGameObjects.Add(winTrigger);
            patrolenemy = (PatrolEnemy)EnemyFactory.CreateEnemy(EnemyFactory.Enemy.patrol, new Vector2(50, 180), false, patrolenemy, new Vector2(100, 180), new Vector2(800, 180), new Vector2 (120, 0));
            ActiveGameObjects.Add(patrolenemy);
            PatrolEnemies.Add(patrolenemy);
            patrolenemy = (PatrolEnemy)EnemyFactory.CreateEnemy(EnemyFactory.Enemy.patrol, new Vector2(250, 300), true, patrolenemy, new Vector2(60, 300), new Vector2(750, 300),  new Vector2(75, 0));
            ActiveGameObjects.Add(patrolenemy);
            PatrolEnemies.Add(patrolenemy);
            enemy = (EnemyController)EnemyFactory.CreateEnemy(EnemyFactory.Enemy.normal, new Vector2(800, 300), false, enemy, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
            ActiveGameObjects.Add(enemy);
            NormalEnemies.Add(enemy);
            EnemyController enemy2 = (EnemyController)EnemyFactory.CreateEnemy(EnemyFactory.Enemy.normal, new Vector2(800, 450), true, enemy, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
            ActiveGameObjects.Add(enemy2);
            NormalEnemies.Add(enemy2);
            patrolenemy = (PatrolEnemy)EnemyFactory.CreateEnemy(EnemyFactory.Enemy.patrol, new Vector2(100, 450), false, patrolenemy, new Vector2(450, 450), new Vector2(700, 450),  new Vector2(150, 0));
            ActiveGameObjects.Add(patrolenemy);
            PatrolEnemies.Add(patrolenemy);
        }
        private void UpdateEnemies()
        {
            foreach (EnemyController enemy in NormalEnemies)
            {
                float DistanceY = Math.Abs(player.Transform.Position.Y - enemy.Transform.Position.Y);
                float SumHalfHeights = player.Render.Size.Y / 2 + enemy.Render.Size.Y / 2;

                if (DistanceY <= SumHalfHeights)
                {
                    if (enemy.IsFacingRight && enemy.Transform.Position.X < player.Transform.Position.X)
                    {
                        enemy.Shoot();
                    }
                    else if (!enemy.IsFacingRight && enemy.Transform.Position.X > player.Transform.Position.X)
                    {
                        enemy.Shoot();
                    }
                }
            }
            foreach (PatrolEnemy enemy in PatrolEnemies)
            {
                float DistanceY = Math.Abs(player.Transform.Position.Y - enemy.Transform.Position.Y);
                float SumHalfHeights = player.Render.Size.Y / 2 + enemy.Render.Size.Y / 2;

                //if player is in range, shoot
                if (DistanceY <= SumHalfHeights)
                {
                    if (enemy.IsFacingRight && enemy.Transform.Position.X < player.Transform.Position.X)
                    {
                        enemy.Shoot();
                    }
                    else if (!enemy.IsFacingRight && enemy.Transform.Position.X > player.Transform.Position.X)
                    {
                        enemy.Shoot();
                    }
                    else
                    {
                        enemy.Patrol();
                    }
                }
                else
                {
                    enemy.Patrol();
                }
            }


        }
    }
}
