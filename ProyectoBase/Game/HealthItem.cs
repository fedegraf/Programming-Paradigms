
namespace Game
{
    public class HealthItem : GameObject
    {
        public HealthItem() : base()
        {
            Collider = new Collider("Health", this);
            Transform = new Transform();
            Transform.Scale = new Vector2(0.05f, 0.05f);
            Render = new Renderer(Engine.GetTexture("Textures/Levels/aid-kid.png"), Transform);
        }

    }
}