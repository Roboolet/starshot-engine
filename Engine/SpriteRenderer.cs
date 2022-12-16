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

        protected override void OnCreate()
        {
            Debug.WriteLine("Created SpriteRenderer :3");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }
    }
}
