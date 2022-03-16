namespace Game
{
    public interface IBullet
    {
        float Speed { get; }
        int Damage { get; }
        FactoryBullets.bullets Type { get; }
        event System.Action<IBullet> OnDeactivate;
        float CoolingTime { get; }
        float CurrentCoolingTime { get; }
        void Update();
        void Deactivate();
        void Instantiate(Vector2 Position);
        void Activate(Vector2 Position, bool right, string Layer);
    }
}
