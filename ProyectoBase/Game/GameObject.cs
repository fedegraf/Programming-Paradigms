namespace Game
{
    public abstract class GameObject
    {
        private Transform transform;
        private Renderer render;
        private Collider collider;
        public bool isActive = true;

        public GameObject(Transform transform, Renderer render, Collider collider)
        {
            Transform = transform;
            Render = render;
            Collider = collider;
        }
        public GameObject()
        {
            Transform = transform;
            Render = render;
            Collider = collider;
        }

        public Transform Transform { get => transform; set => transform = value; }
        public Renderer Render { get => render; set => render = value; }
        public Collider Collider { get => collider; set => collider = value; }

        public virtual void Update()
        {

        }
    }
}
