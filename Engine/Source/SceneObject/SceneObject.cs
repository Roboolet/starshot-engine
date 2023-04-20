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

            AddToScene();
        }

        /// <summary>
        /// Create SceneObject from SceneData
        /// </summary>
        /// <param name=""></param>
        public SceneObject(SceneObject _parent, SceneObjectData _data)
        {
            parent = _parent;
            SceneObjectData comData = _data;
            
            // instantiate as prefab (if appliccable)
            if(!string.IsNullOrEmpty(_data.prefabID) && PrefabLibrary.PrefabExists(_data.prefabID))
            {
                comData = PrefabLibrary.CombineObjects(_data, _data.prefabID);
            }

            // set name
            name = comData.name;
            Debug.WriteLine(" ++ Creating SceneObject: " + name);

            // PRS
            if(comData.position != null) Position = new Vector3( comData.position[0], comData.position[1], 0);
            Rotation = comData.rotation;
            if (comData.scale != null) Scale = new Vector3(comData.scale[0], comData.scale[1], 1);

            // create components
            if (comData.components != null)
            {
                for (int i = 0; i < comData.components.Length; i++)
                {
                    SceneObjectComponentData socd = comData.components[i];
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
            if (comData.children != null)
            {
                for (int i = 0; i < comData.children.Length; i++)
                {
                    children.Add(new SceneObject(this, comData.children[i]));
                }
            }
        }

        public void AddToScene(Scene _scene = null)
        {
            // add to scene
            Enabled = true;

            if (_scene == null)
            {
                scene = Main.instance.currentScene;
            }
            else scene = _scene;

            scene.AddSceneObject(this);

            // add all children too
            for(int i=0; i < children.Count; i++)
            {
                children[i].AddToScene(scene);
            }
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
        protected List<Renderer> components = new List<Renderer>();

        public void AddComponent(ref Type type, SceneObjectComponentData componentData = null)
        {
            Renderer obj = (Renderer)Activator.CreateInstance(type);
            if (obj == null) return;

            obj.InitializeValues(this, componentData);

            components.Add(obj);
        }

        public T AddComponent<T>(SceneObjectComponentData componentData = null) where T : Renderer
        {
            T obj = Activator.CreateInstance<T>();
            if (obj == null) return null;

            obj.InitializeValues(this, componentData);

            components.Add(obj);
            return obj;
        }

        public Renderer GetComponent<T>(ref T type) where T : Renderer
        {
            for (int i = 0; i < components.Count; i++)
            {
                Renderer temp = components[i];
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
