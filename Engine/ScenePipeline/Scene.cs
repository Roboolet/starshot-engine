using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.ScenePipeline
{
    public class Scene
    {
        public readonly string name;
        protected readonly SceneData source;
        public SceneCamera camera;

        List<SceneObject> hierarchy = new List<SceneObject>();

        public Scene(SceneData _source)
        {
            source = _source;
            name = _source.name;
        }

        /// <summary>
        /// Fill in the scene using the data from the source file
        /// </summary>
        public virtual void Populate()
        {
            Debug.WriteLine("Populating Scene...");

            // setup camera
            camera = new SceneCamera();
            camera.position.X = source.camPosition[0];
            camera.position.Y = source.camPosition[1];
            camera.zRotation = source.camRotation;

            // create SceneObjects and components
            for(int i = 0; i < source.objects.Length; i++)
            {
                hierarchy.Add(new SceneObject(source.objects[i]));
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            for(int i =0; i < hierarchy.Count; i++)
            {
                SceneObject obj = hierarchy[i];
                if(obj.Enabled) obj.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            // TODO: draw all Renderers within the hierarchy
        }

        /// <summary>
        /// Create a new instance of the given SceneObject
        /// </summary>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public SceneObject CreateSceneObject(SceneObject source, Vector3 position, SceneObject parent = null)
        {
            return null;
        }

        /// <summary>
        /// Returns the first SceneObject in the scene hierarchy that matches the query
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SceneObject Find(string query, SceneObject searchOrigin = null, FindMode findMode = FindMode.Exact)
        {
            return null;
        }
        /// <summary>
        /// Returns all SceneObjects in the scene that match the query
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SceneObject[] FindAll(string query, SceneObject searchOrigin = null, FindMode findMode = FindMode.Exact)
        {
            return null;
        }
        public enum FindMode{Exact, StartsWith, Contains}
    }
}
