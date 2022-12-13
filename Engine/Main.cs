using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarshotEngine.ScenePipeline;
using System;
using System.Diagnostics;

namespace StarshotEngine
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Main instance;

        private Scene[] scenes;
        public Scene currentScene {get; private set;}
        //private SceneObject[] cachedPrefabs;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            instance = this;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SceneData data = Content.Load<SceneData>("exampleScene");
            Debug.WriteLine(data.name);
            for (int i = 0; i < data.objects.Length; i++)
            {
                Debug.WriteLine(data.objects[i].name);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(currentScene != null)currentScene.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            if (currentScene != null) currentScene.Draw(gameTime);
        }

        public Scene LoadScene(string name)
        {
            return null;
        }
    }
}
