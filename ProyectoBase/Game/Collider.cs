namespace Game
{
    public class Collider
    {
        private string layer;
        private GameObject thisGameObject;
        public string Layer { get => layer; set => layer = value; }
        public GameObject ThisGameObject { get => thisGameObject; set => thisGameObject = value; }
        public Collider(string layer, GameObject gameObject)
        {
            Layer = layer;
            thisGameObject = gameObject;
        }
        public Collider(string layer)
        {
            Layer = layer;
        }

        public GameObject CheckCollitions()
        {
            
            foreach (GameObject gameobject in Level.ActiveGameObjects)
            {
                if (gameobject != thisGameObject)
                {
                 bool IsColliding = CollisionsUtilities.isBoxingColliding(gameobject.Transform.Position, gameobject.Render.Size, 
                        thisGameObject.Transform.Position, thisGameObject.Render.Size);
                    if (IsColliding)
                    {
                        return gameobject;
                    }
                }    
            }
            return null;
        }
    }
}
