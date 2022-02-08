using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class GameOverScreen : IScene
    {
        private const float INPUT_DELAY_TIME = 0.2f;

        private string backgroundTexturePath;
        private Button playButton;
        private Button quitButton;
        private Button selectedButton;
        private float currentInputDelayTime;

        public GameManager.GameState Id => GameManager.GameState.GameOverScreen;
        public string BackgroundTexturePath { get => backgroundTexturePath; set => backgroundTexturePath = value; }
        public GameOverScreen(string backgroundTexturePath)
        {
            BackgroundTexturePath = backgroundTexturePath;
        }

        public void Initialize()
        {
            playButton = new Button(new Vector2(200, 400), 0.35f, "Png/Buttons/Retry.png", "Png/Buttons/RetrySelected.png");
            quitButton = new Button(new Vector2(550, 410), 0.25f, "Png/Buttons/Exit.png", "Png/Buttons/ExitLoseSelected.png");

            playButton.AssignButtons(quitButton, quitButton);
            quitButton.AssignButtons(playButton, playButton);

            SelectButton(playButton);

        }
        public void Update()
        {
            currentInputDelayTime += Time.deltaTime;


            playButton.Update();
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

            if (Engine.GetKey(Keys.SPACE) && currentInputDelayTime >= INPUT_DELAY_TIME)
            {
                currentInputDelayTime = 0;
                PressSelectedButton();
            }
        }
        public void Render()
        {
            Engine.Draw(backgroundTexturePath, 0, 0, 0.52f, 0.52f);

            playButton.Render();
            quitButton.Render();
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

        private void PressSelectedButton()
        {
            if (selectedButton != null)
            {
                if (selectedButton == playButton)
                {
                    GameManager.Instance.Restart();
                }
                else if (selectedButton == quitButton)
                {
                   GameManager.Instance.ExitGame();
                }
            }
        }
    }

}