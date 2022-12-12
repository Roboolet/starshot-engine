using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine
{
    public class Scene
    {
        public readonly string fileName;
        public readonly string name;
        public SceneCamera camera;

        List<SceneObject> hierarchy;

        public Scene(string sourceFileJSON)
        {
            fileName = sourceFileJSON;
        }

        public void Populate()
        {
            camera = new SceneCamera();
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < hierarchy.Count; i++)
            {
                SceneObject obj = hierarchy[i];
                if(obj.Enabled) obj.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
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
