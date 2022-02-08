using System;
using System.Collections.Generic;

namespace Game
{
    public class LevelScene
    {
        private static List<Tile> listOfTiles = new List<Tile>();
        private static List<Bullet> listOfBulllets = new List<Bullet>();
        private PlayerController player;

        public void Initialize ()
        {
            var PlayerData = new PlayerData()
            {
                Position = new Vector2(50, 0),
                Scale = 0.25f,
                RealHeight = Engine.GetTexture("Textures/Character/idle_1.png").Height,
                RealWidht = Engine.GetTexture("Textures/Character/idle_1.png").Width,
                Speed = 50f,
                Texture = Engine.GetTexture("Textures/Character/idle_1.png")
            };
            player = new PlayerController(PlayerData, CreatePlayerAnimation());
            Texture tileTexture = Engine.GetTexture("Textures/Platform_Tile.png");
            CreateTiles(1, tileTexture, 250, 0);
        }
        public void Update()
        {
            player.checkInput();
            if (!player.CheckCollisitionWithTiles(listOfTiles))
            {
                player.Fall();
            }
            if (listOfBulllets.Count != 0)
            {
                listOfBulllets.ForEach(bullet => bullet.UpdatePosition());
            }

        }
        public void Render()
        {
            Engine.Draw("Textures/Background.png", 0, 0, 0.75f, 0.75f);
            DrawTiles(3);
            player.Render();
            listOfBulllets.ForEach(bullet => bullet.Render());
        }
        private static List<Animation> CreatePlayerAnimation()
        {
            List<Animation> animations = new List<Animation>();


            List<Texture> idleFrames = new List<Texture>();
            for (int i = 1; i <= 1; i++)
            {
                idleFrames.Add(Engine.GetTexture($"Textures/Character/idle_{i}.png"));
            }
            Animation idleAnimation = new Animation("idleAnimation", idleFrames, 1f, true);
            animations.Add(idleAnimation);


            List<Texture> walkframes = new List<Texture>();
            for (int i = 1; i <= 12; i++)
            {
                walkframes.Add(Engine.GetTexture($"Textures/Character/Run Animation/walk_{i}.png"));
            }
            Animation runAnimation = new Animation("runAnimation", walkframes, 0.1f, true);
            animations.Add(runAnimation);


            return animations;
        }
        private static void DrawTiles(int cantTiles)
        {
            listOfTiles.ForEach(tile =>
            Engine.Draw(tile.texture, tile.xToDrawTile, tile.yToDrawTile, 1, 1, 0, tile.Size.X / 2, tile.Size.Y / 2)
            );
        }
        private static void CreateTiles(int cantTiles, Texture texture, int YPosition, int XpositionStart)
        {
            int PositionX = XpositionStart;
            for (int i = 0; i <= cantTiles; i++)
            {
                Tile tile = new Tile(PositionX, YPosition, texture);
                listOfTiles.Add(tile);
                PositionX += texture.Width;
            }
        }
        public static void InstantiateBullet(Vector2 position, Vector2 size)
        {
            Bullet myBullet = new Bullet();
            myBullet.Texture = Engine.GetTexture("Textures/Bullet.png");
            float spawnWidth = position.X + size.X / 2;
            myBullet.Position = new Vector2(spawnWidth, position.Y);
            myBullet.Speed = 150f;
            myBullet.Scale = 0.1f;
            myBullet.Damage = 50;
            listOfBulllets.Add(myBullet);

        }

    }
}
