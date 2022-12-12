using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine
{
    public class SceneCamera : SceneObject
    {
        public float orthographicSize;
        
        //private SpriteRenderer[] renderers;

        // draw what the camera sees to the spritebatch
        public void RenderTo(SpriteBatch batch)
        {
            batch.Begin();
            // render all visible renderers
            batch.End();
        }
    }
}
