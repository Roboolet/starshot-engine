using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine.Rendering
{
    public class SceneCamera : SceneObject
    {
        public float renderScale = 1;
        public int pixelsPerUnit = 16;

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
                    Vector2 screenPos = WorldToScreenPos(spRend.sceneObject.Position);
                    Vector2 scaledTex = new Vector2(tex.Width * renderScale * spRend.sceneObject.Scale.X, tex.Height * renderScale * spRend.sceneObject.Scale.Y);

                    Rectangle drawRect = new Rectangle(
                        new Point((int)(screenPos.X - scaledTex.X / 2), (int)(screenPos.Y - scaledTex.Y / 2)),
                        new Point((int)scaledTex.X, (int)scaledTex.Y));
                    batch.Draw(tex, drawRect, spRend.GetColor());
                }
            }

            batch.End();
        }

        public void AddRenderer(SpriteRenderer renderer)
        {
            renderers.Add(renderer);
        }

        public Vector2 WorldToScreenPos(Vector2 vec)
        {
            Viewport vp = Main.instance.GraphicsDevice.Viewport;
            // get coord, subtract camera, and center in screen.
            // additionally, invert the Y axis
            vec = new Vector2(
                vec.X*pixelsPerUnit*renderScale - Position.X*pixelsPerUnit*renderScale + vp.Width / 2,
                -vec.Y*pixelsPerUnit*renderScale - -Position.Y*pixelsPerUnit*renderScale + vp.Height / 2);
            return vec;
        }
    }
}
