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
        public void Render()
        {
            SpriteBatch batch = new SpriteBatch(Main.instance.GraphicsDevice);

            batch.Begin();
            // render all SpriteRenderers
            for(int i = 0; i < renderers.Count; i++)
            {
                SpriteRenderer spRend = renderers[i];

                // get texture 
                Texture2D tex = spRend.sprite;

                if (tex != null)
                {
                    Rectangle drawRect = new Rectangle(new Point((int)spRend.sceneObject.position.X - tex.Width / 2, (int)spRend.sceneObject.position.Y - tex.Height / 2)
                        , new Point(tex.Width, tex.Height));
                    batch.Draw(tex, drawRect, spRend.GetColor());
                }
            }

            batch.End();
        }

        public void AddRenderer(SpriteRenderer renderer)
        {
            renderers.Add(renderer);
        }
    }
}
