namespace Game
{
    public class Tile : GameObject
    {
        public Tile() : base()
        {
            Collider = new Collider("Tile", this);
            Transform = new Transform();
            Transform.Scale = new Vector2(0.25f, 0.25f);
            Render = new Renderer(Engine.GetTexture("Textures/Levels/Platform_Tile.png"), Transform);
        }
    }
}
