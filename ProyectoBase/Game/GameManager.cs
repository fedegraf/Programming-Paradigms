using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameManager
    {
        public enum GameState
        {
            MainMenu,
            Controls,
            GameOverScreen,
            WinScreen,
            Level
        }
        public List<IScene> scenes = new List<IScene>();
        private WinScreen winScreen;
        private GameOverScreen gameOverSceen;
        private Level level;
        private MainMenu mainMenu;
        private Controls Controls;

        public IScene CurrentScene { get; private set; }

        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }

                return instance;
            }
        }

        public GameState CurrentGameState { get; private set; }
        public void Initialize()
        {
            winScreen = new WinScreen("Png/Screens/Win.png");
            scenes.Add(winScreen);
            winScreen.Initialize();

            gameOverSceen = new GameOverScreen("Png/Screens/Loser.png");
            scenes.Add(gameOverSceen);
            gameOverSceen.Initialize();

            level = new Level();
            scenes.Add(level);
            level.Initialize();

            mainMenu = new MainMenu("Png/Screens/MainMenu.png");
            scenes.Add(mainMenu);
            mainMenu.Initialize();

            Controls = new Controls("Png/Screens/Controls.png");
            scenes.Add(Controls);
            Controls.Initialize();

            ChangeGameState(GameManager.GameState.MainMenu);
        }

        public void Update()
        {
          CurrentScene.Update();
        }

        public void Render()
        {
            CurrentScene.Render();
        }
        public void ChangeGameState(GameState gamestate)
        {
            foreach (IScene scene in scenes)
            {
                if (scene.Id == gamestate)
                {
                    CurrentScene = scene;
                }
            }
        }
        public void ExitGame()
        {
            Environment.Exit(1);
        }
        public void Restart()
        {
            scenes.Remove(level);
            level = new Level();
            scenes.Add(level);
            level.Initialize();
            ChangeGameState(GameState.Level);
        }
    }
}
