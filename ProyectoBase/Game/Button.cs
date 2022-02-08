using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum ButtonState
    {
        Normal,
        Selected
    }


    public class Button
    {
        private ButtonState currentState;
        private string normalTexturePath;
        private string selectedTexturePath;
        private Vector2 position;
        private float scale;

        public Button NextButton { get; private set; }
        public Button PreviousButton { get; private set; }


        public Button(Vector2 initialPosition,float scale,string normalTexturePath, string selectedTexturePath)
        {
            position = initialPosition;
            this.normalTexturePath = normalTexturePath;
            this.selectedTexturePath = selectedTexturePath;
            this.scale = scale;
        }

        public void AssignButtons(Button nexbutton, Button previousButton)
        {
            NextButton = nexbutton;
            PreviousButton = previousButton;
        }

        public void Update()
        {

        }
        public void UnSelect()
        {
            currentState = ButtonState.Normal;
        }

        public void Select()
        {
            currentState = ButtonState.Selected;
        }

        public void Render()
        {
            string texturePath = null;

            switch (currentState)
            {
                case ButtonState.Normal:
                    texturePath = normalTexturePath;
                    break;
                case ButtonState.Selected:
                    texturePath = selectedTexturePath;
                    break;
                default:
                    break;
            }

            Engine.Draw(texturePath, position.X,position.Y,scale,scale,0);
        }
    }

}
