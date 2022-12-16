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

        private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

        // draw what the camera sees to the spritebatch
        public void RenderTo(SpriteBatch batch)
        {
            batch.Begin();
            // render all SpriteRenderers
            for(int i = 0; i < renderers.Count; i++)
            {
                SpriteRenderer spRend = renderers[i];

                // get texture if not already loaded
                Texture2D tex;
                //batch.Draw(tex);
            }

            batch.End();
        }

        public void AddRenderer(SpriteRenderer renderer)
        {
            renderers.Add(renderer);
        }
    }
}
