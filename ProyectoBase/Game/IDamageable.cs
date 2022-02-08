namespace Game
{
    interface IDamageable
    {
        bool isDestroyed { get; }
        event System.Action<IDamageable> OnDestroy;
        void GetDamage(int damage);
        void Destroy();
    }
}
