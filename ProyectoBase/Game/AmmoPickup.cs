
namespace Game
{
    public class AmmoPickup : GameObject
    {
        public AmmoPickup() : base()
        {
            Collider = new Collider("AmmoPickup", this);
            Transform = new Transform();
            Transform.Scale = new Vector2(0.05f, 0.05f);
            Render = new Renderer(Engine.GetTexture("Textures/Levels/explosive-pickup.png"), Transform);
        }

    }
}