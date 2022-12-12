using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine
{
    public class SceneObject
    {
        public string name;
        public Vector2 position;
        public Quaternion rotation;
        public Vector2 scale;

        protected ISceneObjectComponent[] components;
        public List<SceneObject> children;

        public SceneObject(string _name = "NewObject")
        {
            name = _name;
        }

        public void Destroy()
        {
            for(int i = 0; i < components.Length; i++)
            {
                components[i].OnDestroy();
            }
        }

        public void Translate(float x, float y) { }
        public void Rotate(float xEuler, float yEuler, float zEuler) { }
        public ISceneObjectComponent GetComponent<T>(ref T type) where T : ISceneObjectComponent
        {
            for(int i = 0; i < components.Length; i++)
            {
                ISceneObjectComponent temp = components[i];

            }
        }
    }
}
