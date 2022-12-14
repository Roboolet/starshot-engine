using Microsoft.Xna.Framework;
using StarshotEngine.SceneObjectComponent;
using StarshotEngine.ScenePipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected List<SceneObject> children;

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

        public SceneObject(string _name = "NewObject")
        {
            name = _name;
        }

        /// <summary>
        /// Create SceneObject from SceneData
        /// </summary>
        /// <param name=""></param>
        public SceneObject(SceneObjectData data, SceneObject parent = null)
        {
            // set name
            name = data.name;
            Debug.WriteLine(" -- Creating SceneObject: " + name);

            // PRS
            position.X = data.position[0];
            position.Y = data.position[1];
            zRotation = data.rotation;
            scale.X = data.scale[0];
            scale.Y = data.scale[1];

            // create components
            if (data.components != null)
            {
                components = new List<ISceneObjectComponent>();
                for (int i = 0; i < data.components.Length; i++)
                {
                    Debug.WriteLine("Adding component: " + data.components[i].type);
                    Type compType = ComponentLibrary.GetComponentType(data.components[i].type);
                    if (compType != null) { AddComponent(ref compType); }
                }
            }

            // create children
            if (data.children != null)
            {
                children = new List<SceneObject>();
                for (int i = 0; i < data.children.Length; i++)
                {
                    children.Add(new SceneObject(data.children[i], this));
                }
            }

            Enabled = true;
        }

        
        public void AddComponent<T>() where T : ISceneObjectComponent
        {
            T obj = Activator.CreateInstance<T>();
            components.Add(obj);

            // if component is a renderer, add it to camera
        }
        public void AddComponent(ref Type type)
        {
            ISceneObjectComponent obj = (ISceneObjectComponent)Activator.CreateInstance(type);
            components.Add(obj);

            // if component is a renderer, add it to camera
        }

        public void SetParent()
        {
            // should not allow a parent of this object to become a child, and vice versa
        }

        public void Destroy()
        {
            if (components != null)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    components[i].OnDestroy();
                }
            }
            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Destroy();
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            if (components != null)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    components[i].OnUpdate(gameTime);
                }
            }
            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Update(gameTime);
                }
            }
        }

        public void Translate(float x, float y) { }
        public void Rotate(float xEuler, float yEuler, float zEuler) { }
        public ISceneObjectComponent GetComponent<T>(ref T type) where T : ISceneObjectComponent
        {
            for (int i = 0; i < components.Count; i++)
            {
                ISceneObjectComponent temp = components[i];
                if (temp.GetType() == type.GetType())
                {
                    return temp;
                }
            }
            return null;
        }
    }
}
