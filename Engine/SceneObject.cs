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
using MonoGame.Extended;

namespace StarshotEngine
{
    public class SceneObject
    {
        public SceneObject parent { get; set; }
        public Scene scene { get; set; }
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

        #region Position, Rotation, Scale
        public Matrix matrix = Matrix.Identity;
        public Matrix LocalMatrix
        {
            get
            {
                if (parent != null)
                {
                    return matrix * parent.LocalMatrix;
                }
                else return matrix * scene.matrix;
            }
        }

        public Vector3 Position
        {
            get
            {
                return LocalMatrix.Translation;
            }
            set
            {
                matrix *= Matrix.CreateTranslation(value - Position);
            }
        }
        public float Rotation
        {
            get 
            {
                if (parent != null)
                {
                    return MathF.Atan2(matrix.M21, matrix.M11) + parent.Rotation;
                }
                else return MathF.Atan2(matrix.M21, matrix.M11);
            }
            set
            {
                matrix = Matrix.CreateRotationZ(value - Rotation) * matrix;
            }
        }

        public Vector3 Scale
        {
            get 
            {
                Matrix mat = LocalMatrix;
                float lenX = new Vector3(mat.M11, mat.M21, mat.M31).Length();
                float lenY = new Vector3(mat.M12, mat.M22, mat.M32).Length();
                float lenZ = new Vector3(mat.M13, mat.M23, mat.M33).Length();
                return new Vector3(lenX, lenY, lenZ);
            }
            set { matrix = Matrix.CreateScale(value) * matrix; }
        }
        #endregion

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

        public SceneObject(SceneObject _parent = null, string _name = "NewObject")
        {
            name = _name;
            parent = _parent;
            Enabled = true;

            scene = Main.instance.currentScene;
            scene.AddSceneObject(this);
        }

        /// <summary>
        /// Create SceneObject from SceneData
        /// </summary>
        /// <param name=""></param>
        public SceneObject(SceneObject _parent, SceneObjectData _data)
        {
            parent = _parent;
            scene = Main.instance.currentScene;

            // set name
            name = _data.name;
            Debug.WriteLine(" ++ Creating SceneObject: " + name);

            // PRS
            Position = new Vector3( _data.position[0], _data.position[1], 0);
            Rotation = _data.rotation;
            Scale = new Vector3(_data.scale[0], _data.scale[1], 1);

            // create components
            if (_data.components != null)
            {
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
                    children.Add(new SceneObject(this, _data.children[i]));
                }
            }

            Enabled = true;

            // add to scene
            scene.AddSceneObject(this);
        }

        public void Start()
        {
            if (components != null)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    components[i].Start();
                }
            }
            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Start();
                }
            }
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

        #region Components
        protected List<SceneObjectComponent> components = new List<SceneObjectComponent>();

        public void AddComponent(ref Type type, SceneObjectComponentData componentData = null)
        {
            SceneObjectComponent obj = (SceneObjectComponent)Activator.CreateInstance(type);
            if (obj == null) return;

            obj.InitializeValues(this, componentData);

            components.Add(obj);
        }

        public T AddComponent<T>(SceneObjectComponentData componentData = null) where T : SceneObjectComponent
        {
            T obj = Activator.CreateInstance<T>();
            if (obj == null) return null;

            obj.InitializeValues(this, componentData);

            components.Add(obj);
            return obj;
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
        #endregion

        #region Hierarchy & Children
        protected List<SceneObject> children = new List<SceneObject>();
        public enum FindMode { Exact, StartsWith, Contains }

        public void SetParent()
        {
            // should not allow a parent of this object to become a child, and vice versa
        }

        public SceneObject GetChild(int index) => children[index];
        public void AddChild(SceneObject obj)
        {
            obj.parent = this;
            children.Add(obj);
        }

        public int ChildCount => children.Count;

        public SceneObject Find(string query, FindMode findMode = FindMode.Exact) => SceneObject.Find(this, query, findMode);

        /// <summary>
        /// Returns the first SceneObject in the scene hierarchy that matches the query
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SceneObject Find(SceneObject searchOrigin, string query, FindMode findMode = FindMode.Exact)
        {
            // currently only works on root level objects or direct children
            SceneObject output = null;

            for (int i = 0; i < searchOrigin.ChildCount; i++)
            {
                SceneObject sob = searchOrigin.GetChild(i);
                if(sob.name == query)
                {
                    output = sob;
                    break;
                }
            }
            return output;
        }
        /// <summary>
        /// Returns all SceneObjects in the scene that match the query
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SceneObject[] FindAll(SceneObject searchOrigin, string query, FindMode findMode = FindMode.Exact)
        {
            return null;
        }
        #endregion
    }
}
