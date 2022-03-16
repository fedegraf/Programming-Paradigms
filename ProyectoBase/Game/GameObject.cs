namespace Game
{
    public abstract class GameObject
    {
        private Transform transform;
        private Renderer render;
        private Collider collider;
        public bool isActive = true;

        public GameObject()
        {
            transform = new Transform();
            render = new Renderer(Engine.GetTexture("Textures/null_texture.png"), transform);
            collider = new Collider("null", this);
        }
        public GameObject(Vector2 position, float rotation, Vector2 scale, Texture texture = null, string layer = null)
        {
            Transform = new Transform(position, rotation, scale);
            Render = new Renderer(texture, Transform);
            Collider = new Collider(layer, this);
        }

        public Transform Transform { get => transform; set => transform = value; }
        public Renderer Render { get => render; set => render = value; }
        public Collider Collider { get => collider; set => collider = value; }

        public virtual void Update()
        {

        }
    }
}
