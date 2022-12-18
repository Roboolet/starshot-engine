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
        public SceneObject parent { get; set; }
        public SceneObject root
        {
            get
            {
                SceneObject iter = parent;
                while(iter != null)
                {
                    if (iter.parent != null) iter = iter.parent;
                    else break;
                }
                return iter;
            }
        }
        public string name;

        Vector2 _Position;
        public Vector2 Position
        {
            get { return GetPosition(); }
            set { _Position = value; }
        }

        float _Rotation;
        public float Rotation
        {
            get { return GetRotation(); }
            set { _Rotation = value; }
        }

        Vector2 _Scale;
        public Vector2 Scale
        {
            get { return GetScale(); }
            set { _Scale = value; }
        }

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
        public SceneObject(SceneObjectData _data, SceneObject _parent = null)
        {
            parent = _parent;

            // set name
            name = _data.name;
            Debug.WriteLine(" ++ Creating SceneObject: " + name);

            // PRS
            Position = new Vector2( _data.position[0], _data.position[1]);
            Rotation = _data.rotation;
            Scale = new Vector2(_data.scale[0], _data.scale[1]);

            // create components
            if (_data.components != null)
            {
                components = new List<SceneObjectComponent>();
                for (int i = 0; i < _data.components.Length; i++)
                {
                    SceneObjectComponentData socd = _data.components[i];
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
            if (_data.children != null)
            {
                children = new List<SceneObject>();
                for (int i = 0; i < _data.children.Length; i++)
                {
                    children.Add(new SceneObject(_data.children[i], this));
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

        /// <summary>
        /// Gets the sum position of this object
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            Vector2 sum = _Position;
            SceneObject parentIteration = parent;
            while(parentIteration != null)
            {
                sum = sum + parentIteration._Position;
                parentIteration = parentIteration.parent;
            }        

            return sum;
        }

        /// <summary>
        /// Gets the sum scale of this object
        /// </summary>
        /// <returns></returns>
        public Vector2 GetScale()
        {
            Vector2 sum = _Scale;
            SceneObject parentIteration = parent;
            while (parentIteration != null)
            {
                sum = sum * parentIteration._Scale;
                parentIteration = parentIteration.parent;
            }

            return sum;
        }

        /// <summary>
        /// Gets the sum rotation of this object
        /// </summary>
        /// <returns></returns>
        public float GetRotation()
        {
            float sum = _Rotation;
            SceneObject parentIteration = parent;
            while (parentIteration != null)
            {
                sum = sum + parentIteration._Rotation;
                parentIteration = parentIteration.parent;
            }

            return sum;
        }

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
