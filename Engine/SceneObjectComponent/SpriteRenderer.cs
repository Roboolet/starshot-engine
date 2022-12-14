using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.SceneObjectComponent
{
    public class SpriteRenderer : ISceneObjectComponent
    {
        public SpriteRenderer()
        {
            Debug.WriteLine("Created SpriteRenderer");
        }

        public void OnUpdate(GameTime gameTime)
        {
            Debug.WriteLine("hi :3");
        }
    }
}
