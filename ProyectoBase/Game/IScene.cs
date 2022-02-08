namespace Game
{
    public interface IScene
    {
        GameManager.GameState Id { get; }
        string BackgroundTexturePath { get; set; }
        
        void Initialize();
        void Render();
        void Update();
    }
}
