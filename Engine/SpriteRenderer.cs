using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine
{
    public class SpriteRenderer : SceneObjectComponent
    {
        public string spritePath;
        public float[] color;

        public void OnStart()
        {
            Debug.WriteLine("Created SpriteRenderer :3");
        }

        public void OnUpdate(GameTime gameTime)
        {
        }
    }
}
