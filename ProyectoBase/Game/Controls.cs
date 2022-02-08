using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Controls : IScene
    {
        private string backgroundTexturePath;
        public GameManager.GameState Id => GameManager.GameState.Controls;
        public string BackgroundTexturePath { get => backgroundTexturePath; set => backgroundTexturePath = value; }

        public Controls(string backgroundTexturePath)
        {
            BackgroundTexturePath = backgroundTexturePath;
        }

        public void Initialize()
        {

        }
        public void Update()
        {
            if (Engine.GetKey(Keys.ESCAPE))
            {
                GameManager.Instance.ChangeGameState(GameManager.GameState.MainMenu);
            }
        }
        public void Render()
        {
            Engine.Draw(backgroundTexturePath, 0, 0, 0.55f, 0.55f);
        }
    }
}
