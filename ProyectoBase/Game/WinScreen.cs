using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class WinScreen : IScene
    {
        private const float INPUT_DELAY_TIME = 0.2f;

        private string backgroundTexturePath;
        private Button quitButton;
        private Button menuButton;
        private Button selectedButton;
        private float currentInputDelayTime;

        public GameManager.GameState Id => GameManager.GameState.WinScreen;
        public string BackgroundTexturePath { get => backgroundTexturePath; set => backgroundTexturePath = value; }
        public WinScreen(string backgroundTexturePath)
        {
            BackgroundTexturePath = backgroundTexturePath;
        }

        public void Initialize()
        {
            quitButton = new Button(new Vector2(250, 400), 0.25f, "Png/Buttons/Exit.png", "Png/Buttons/ExitWinSelected.png");
            menuButton = new Button(new Vector2(550, 400), 0.25f, "Png/Buttons/Menu.png", "Png/Buttons/MenuSelected.png");

            quitButton.AssignButtons(menuButton, menuButton);
            menuButton.AssignButtons(quitButton, quitButton);

            SelectButton(quitButton);
        }
        public void Update()
        {
            currentInputDelayTime += Time.deltaTime;


            menuButton.Update();
            quitButton.Update();

            if ((Engine.GetKey(Keys.UP) || Engine.GetKey(Keys.W)) && currentInputDelayTime >= INPUT_DELAY_TIME)
            {
                currentInputDelayTime = 0;
                SelectButton(selectedButton.PreviousButton);
            }

            if ((Engine.GetKey(Keys.DOWN) || Engine.GetKey(Keys.S)) && currentInputDelayTime >= INPUT_DELAY_TIME)
            {
                currentInputDelayTime = 0;
                SelectButton(selectedButton.NextButton);
            }
            if (Engine.GetKey(Keys.SPACE))
            {
                if (selectedButton == menuButton)
                {
                    GameManager.Instance.ChangeGameState(GameManager.GameState.MainMenu);
                }
                else if (selectedButton == quitButton)
                {
                    GameManager.Instance.ExitGame();
                }
            }
        }
        private void SelectButton(Button button)
        {
            if (selectedButton != null)
            {
                selectedButton.UnSelect();
            }
            selectedButton = button;
            selectedButton.Select();
        }
        public void Render()
        {
            Engine.Draw(backgroundTexturePath,0, 0, 0.52f, 0.52f);
            quitButton.Render();
            menuButton.Render();
        }
    }
}
