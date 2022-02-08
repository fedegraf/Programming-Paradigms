namespace Game
{
    public class Renderer
    {
        protected Texture texture;
        protected float realheight;
        protected float realwidht;
        protected Vector2 size;
        protected float offSetX;
        protected float offSetY;
        public float RealHeight { get => realheight; set => realheight = value; }
        public float RealWidht { get => realwidht; set => realwidht = value; }
        public float OffSetY { get => offSetY; set => offSetY = value; }
        public float OffSetX { get => offSetX / 2; set => offSetX = value; }
        public Vector2 Size { get => size; set => size = value; }
        public Texture Texture { get => texture; set => texture = value; }

        public Renderer(Texture texture, Transform transform)
        {
            this.Texture = texture;
            RealWidht = texture.Width;
            RealHeight = texture.Height;
            Size = new Vector2(RealWidht * transform.Scale.X, RealHeight * transform.Scale.Y);
            OffSetX = size.X / 2;
            OffSetY = size.Y / 2;
        }
        public void Render(Transform transform)
        {
            Engine.Draw(texture, transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y,
                transform.Rotation, OffSetX, OffSetY);
        }
    }
}
