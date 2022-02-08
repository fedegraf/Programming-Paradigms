using System;

namespace Game
{
    class WinTrigger : GameObject
    {
        public WinTrigger () : base()
        {
            Collider = new Collider("Win Trigger", this);
            Transform = new Transform();
            Transform.Scale = new Vector2(0.25f, 0.25f);
            Render = new Renderer(Engine.GetTexture("Textures/Levels/Door.png"), Transform);
        }

        public virtual void update()
        {

        }
    }
}
