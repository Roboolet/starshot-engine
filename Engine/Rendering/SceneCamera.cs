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
        public int pixelsPerUnit = 16;

        private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

        // draw what the camera sees to the spritebatch
        public void Render()
        {
            SpriteBatch batch = new SpriteBatch(Main.instance.GraphicsDevice);
            batch.Begin(SpriteSortMode.FrontToBack, transformMatrix:matrix);
            // render all SpriteRenderers
            for(int i = 0; i < renderers.Count; i++)
            {
                SpriteRenderer spRend = renderers[i];

                // get texture 
                Texture2D tex = spRend.sprite;

                if (tex != null)
                {                    
                    // get values
                    Vector2 screenPos = WorldToScreenPos(spRend.sceneObject.Position);
                    Vector3 scale = spRend.sceneObject.Scale;
                    float ang = spRend.sceneObject.Rotation;

                    // scaling
                    Vector2 scaledTex = new Vector2(tex.Width * scale.X, tex.Height * scale.Y);

                    Rectangle drawRect = new Rectangle(
                        new Point((int)(screenPos.X), (int)(screenPos.Y)),
                        new Point((int)scaledTex.X, (int)scaledTex.Y));
                    batch.Draw(tex, drawRect, null, spRend.GetColor(), ang, new Vector2(tex.Width/2, tex.Height/2), SpriteEffects.None, spRend.drawOrder);
                    
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
                vec.X*pixelsPerUnit - Position.X*pixelsPerUnit + vp.Width / 2,
                -vec.Y*pixelsPerUnit - -Position.Y*pixelsPerUnit + vp.Height / 2);
            return newVec;
        }
    }
}
