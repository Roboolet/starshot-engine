using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using StarshotEngine.ScenePipeline;
using StarshotEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StarshotEngine
{
    public class SceneObject
    {
        public string name;
        public Vector2 position;
        public float zRotation;
        public Vector2 scale;

        protected List<SceneObjectComponent> components;
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
            Debug.WriteLine(" ++ Creating SceneObject: " + name);

            // PRS
            position.X = data.position[0];
            position.Y = data.position[1];
            zRotation = data.rotation;
            scale.X = data.scale[0];
            scale.Y = data.scale[1];

            // create components
            if (data.components != null)
            {
                components = new List<SceneObjectComponent>();
                for (int i = 0; i < data.components.Length; i++)
                {
                    SceneObjectComponentData socd = data.components[i];
                    Debug.WriteLine(" ++ Adding component: " + socd.type);
                    Type compType = ComponentLibrary.GetComponentType(socd.type);

                    if (compType != null)
                    {
                        // add it to the list
                        AddComponent(ref compType, socd);
                    }
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
        public void AddComponent(ref Type type, SceneObjectComponentData componentData = null)
        {
            SceneObjectComponent obj = (SceneObjectComponent)Activator.CreateInstance(type);
            if (obj == null) return;

            obj.InitializeValues(this, componentData);

            components.Add(obj);

            // if component is a renderer, add it to camera
            // TODO: add StackRenderer to this
            if(type == typeof(SpriteRenderer))
            {
                Main.instance.currentScene.camera.AddRenderer((SpriteRenderer)obj);
            }

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
                    components[i].Destroy();
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
                    components[i].Update(gameTime);
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
        public SceneObjectComponent GetComponent<T>(ref T type) where T : SceneObjectComponent
        {
            for (int i = 0; i < components.Count; i++)
            {
                SceneObjectComponent temp = components[i];
                if (temp.GetType() == type.GetType())
                {
                    return temp;
                }
            }
            return null;
        }
    }
}
