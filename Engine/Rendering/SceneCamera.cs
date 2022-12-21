using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            batch.Begin(SpriteSortMode.FrontToBack);
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
                        new Point((int)(screenPos.X), (int)(screenPos.Y)),
                        new Point((int)scaledTex.X, (int)scaledTex.Y));
                    batch.Draw(tex, drawRect, null, spRend.GetColor(), spRend.sceneObject.Rotation, new Vector2(tex.Width/2, tex.Height/2), SpriteEffects.None, spRend.drawOrder);
                    
                }
            }

            batch.End();
        }

        public void AddRenderer(SpriteRenderer renderer)
        {
            renderers.Add(renderer);
        }

        public Vector2 WorldToScreenPos(Vector3 vec)
        {
            Viewport vp = Main.instance.GraphicsDevice.Viewport;
            // get coord, subtract camera, and center in screen.
            // additionally, invert the Y axis
            Vector2 newVec = new Vector2(
                vec.X*pixelsPerUnit*renderScale - Position.X*pixelsPerUnit*renderScale + vp.Width / 2,
                -vec.Y*pixelsPerUnit*renderScale - -Position.Y*pixelsPerUnit*renderScale + vp.Height / 2);
            return newVec;
        }
    }
}
