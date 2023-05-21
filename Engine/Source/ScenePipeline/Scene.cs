using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using StarshotEngine.Rendering;
using StarshotEngine.ScenePipeline;

namespace StarshotEngine.ScenePipeline
{
    public class Scene
    {
        public readonly string name;
        protected readonly SceneData source;
        public SceneCamera camera;

        public SceneObject root { get; private set; }

        public Matrix matrix = Matrix.Identity;

        public Scene(SceneData _source)
        {
            source = _source;
            name = _source.name;
        }

        bool isPopulated = false;
        /// <summary>
        /// Fill in the scene using the data from the source file
        /// </summary>
        public virtual void Populate()
        {
            Debug.WriteLine("Populating Scene...");

            // create root
            root = new SceneObject(null, "root");

            // setup camera
            camera = new SceneCamera();
            camera.Position = new Vector3(source.camPosition[0], source.camPosition[1], 0);
            camera.Rotation = source.camRotation;

            // create SceneObjects and components
            for(int i = 0; i < source.objects.Length; i++)
            {
                new SceneObject(root, source.objects[i]).AddToScene(this);
            }

            // call OnStart
            for (int i = 0; i < root.ChildCount; i++)
            {
                root.GetChild(i).Start();
            }
            isPopulated = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            for(int i =0; i < root.ChildCount; i++)
            {
                SceneObject obj = root.GetChild(i);
                if(obj.Enabled) obj.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            // TODO: draw all Renderers within the hierarchy
            camera.Render();
        }

        public void AddSceneObject(SceneObject obj)
        {
            if (root != null)
            {
                root.AddChild(obj);
            }
            if (isPopulated) obj.Start();
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
    }
}
