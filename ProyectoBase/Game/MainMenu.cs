using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class MainMenu : IScene
    {
        private const float INPUT_DELAY_TIME = 0.2f;

        private string backgroundTexturePath;
        private Button playButton;
        private Button ControlsButton;
        private Button quitButton;
        private Button selectedButton;
        private float currentInputDelayTime;
        public GameManager.GameState Id => GameManager.GameState.MainMenu;

        public string BackgroundTexturePath { get => backgroundTexturePath; set => backgroundTexturePath = value; }
        public MainMenu(string backgroundTexturePath)
        {
            BackgroundTexturePath = backgroundTexturePath;

        }

        public void Initialize()
        {
            playButton = new Button(new Vector2(550,250),0.5f, "Png/Buttons/Play.png", "Png/Buttons/PlaySelected.png");
            ControlsButton = new Button(new Vector2(550, 300), 0.5f, "Png/Buttons/Controls.png", "Png/Buttons/ControlsSelected.png");
            quitButton = new Button(new Vector2(550, 350), 0.5f, "Png/Buttons/Quit.png", "Png/Buttons/QuitSelected.png");

            playButton.AssignButtons(ControlsButton, quitButton);
            ControlsButton.AssignButtons(quitButton, playButton);
            quitButton.AssignButtons(playButton, ControlsButton);

            SelectButton(playButton);
        }
        public void Update()
        {
            currentInputDelayTime += Time.deltaTime;


            playButton.Update();
            ControlsButton.Update();
            quitButton.Update();

            if ((Engine.GetKey(Keys.UP) || Engine.GetKey(Keys.W))  && currentInputDelayTime >= INPUT_DELAY_TIME )
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
            Engine.Draw(backgroundTexturePath, 0, 0, 0.55f, 0.55f);

         playButton.Render();
         ControlsButton.Render();
         quitButton.Render();
        }

        private void SelectButton(Button button)
        {
            if(selectedButton != null)
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
                    GameManager.Instance.ChangeGameState(GameManager.GameState.Level);
                }
                else if (selectedButton == ControlsButton)
                {
                    GameManager.Instance.ChangeGameState(GameManager.GameState.Controls);
                }
                else if (selectedButton == quitButton)
                {
                    GameManager.Instance.ExitGame();
                }
            }
        }
    }

}
