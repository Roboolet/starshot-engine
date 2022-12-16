using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.SceneObjectComponent
{
    public class SpriteRenderer : ISceneObjectComponent
    {
        public string spritePath { get; set; }

        public SpriteRenderer()
        {
            Debug.WriteLine("Created SpriteRenderer :3");
        }

        public void OnUpdate(GameTime gameTime)
        {
        }
    }
}
