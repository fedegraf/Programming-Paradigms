using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public delegate void GameObjectDelegate(GameObject gameObject);
    public class Collider
    {
        private string layer;
        private GameObject thisGameObject;
        public string Layer { get => layer; set => layer = value; }
        public GameObject ThisGameObject { get => thisGameObject; set => thisGameObject = value; }
        public GameObjectDelegate OnCollition;
        public Collider(string layer, GameObject gameObject)
        {
            Layer = layer;
            thisGameObject = gameObject;
        }
        public Collider(string layer)
        {
            Layer = layer;
        }

        public void CheckCollitions()
        {

            foreach (GameObject gameobject in Level.ActiveGameObjects.ToList())
            {
                //Check if every object active in scene, except for yourself, is colliding with you
                if (gameobject != thisGameObject)
                {
                 bool IsColliding = CollisionsUtilities.isBoxingColliding(gameobject.Transform.Position, gameobject.Render.Size, 
                        thisGameObject.Transform.Position, thisGameObject.Render.Size);
                    if (IsColliding)
                    {
                        // If is colliding, invoke OnCollition Event
                        OnCollition?.Invoke(gameobject);
                    }
                }    
            }
        }
    }
}
