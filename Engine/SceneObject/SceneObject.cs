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
        public float zRotation;
        public Vector2 scale;

        protected List<ISceneObjectComponent> components;
        public List<SceneObject> children;

        public event EventHandler EnabledChanged;

        bool _Enabled;
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                EnabledChanged?.Invoke(this, null);
            }
        }

        public bool HasRenderer { get; private set; }

        public SceneObject(string _name = "NewObject")
        {
            name = _name;
        }

        public void Destroy()
        {
            for(int i = 0; i < components.Count; i++)
            {
                components[i].OnDestroy();
            }
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].OnUpdate();
            }
        }

        public void Translate(float x, float y) { }
        public void Rotate(float xEuler, float yEuler, float zEuler) { }
        public ISceneObjectComponent GetComponent<T>(ref T type) where T : ISceneObjectComponent
        {
            for(int i = 0; i < components.Count; i++)
            {
                ISceneObjectComponent temp = components[i];
                if(temp.GetType() == type.GetType())
                {
                    return temp;
                }
            }
            return null;
        }
        public void AddComponent<T>(ref T type) where T : ISceneObjectComponent
        {
            // TODO: integrate ComponentLib
            // components.Add();

            // if component is a renderer, enable the HasRenderer flag
        }
    }
}
