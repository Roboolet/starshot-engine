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

            // borderless fullscreen
            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // initialize component library
            ComponentLibrary.Initialize();
            
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(currentScene != null) currentScene.Update(gameTime);

            if (_graphics.IsFullScreen && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            if (currentScene != null) currentScene.Draw(gameTime);
        }

        public void LoadScene(string name)
        {
            currentScene = new Scene(Content.Load<SceneData>(name));
            currentScene.Populate();
        }

        public void LoadLdtk(string name)
        {

        }
    }
}
