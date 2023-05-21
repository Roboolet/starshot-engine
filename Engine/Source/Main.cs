using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarshotEngine.ScenePipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StarshotEngine
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Main instance;

        private Dictionary<string, SceneData> scenes = new Dictionary<string, SceneData>();

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
            
            // initialize component library
            ComponentClassLibrary.Initialize();

            // load project data
            ProjectData project = Content.Load<ProjectData>("game");
            Debug.WriteLine("Setting up prefabs...");
            for (int i =0; i < project.prefabs.Length; i++)
            {
                PrefabLibrary.AddPrefab(project.prefabs[i]);
            }

            Debug.WriteLine("Setting up scenes...");
            for (int i =0; i < project.scenes.Length; i++)
            {
                SceneData d = project.scenes[i];
                scenes.Add(d.name, d);
            }
            Debug.WriteLine("Finished loading project file");

            // borderless fullscreen
            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
            
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
            currentScene = new Scene(scenes[name]);
            currentScene.Populate();
        }
    }
}
