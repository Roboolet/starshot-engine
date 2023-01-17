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

        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        List<StackRenderer> stackRenderers = new List<StackRenderer>();

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
                Texture2D tex = spRend.GetSprite();
                RenderSingleSprite(batch, spRend, tex, Point.Zero);
            }

            // render all StackRenderers
            for (int i = 0; i < stackRenderers.Count; i++)
            {
                StackRenderer spRend = stackRenderers[i];

                // loop over each sprite in the stack
                for(int spi = 0; spi < spRend.spriteCount; spi++)
                {
                    // get texture 
                    Texture2D tex = spRend.GetSprite(spi);
                    Point offset = Point.Zero;

                    if (!spRend.followCamera)
                    {
                        Vector2 vec = new Vector2(spRend.basisVector[0], spRend.basisVector[1]);
                        offset = new Point((int)Math.Round(spi * vec.X * spRend.magnitude), (int)Math.Round(spi * vec.Y * spRend.magnitude));
                    }
                    else
                    {
                        // render based on camera position in relation to the renderer
                        float diffY = Position.Y - spRend.sceneObject.Position.Y;
                        float diffX = spRend.sceneObject.Position.X - Position.X;

                        Vector2 vec = new Vector2(Math.Clamp(diffX * spRend.magnitude, -spRend.maximumCameraPerspective, spRend.maximumCameraPerspective),
                            Math.Clamp(diffY * spRend.magnitude, -spRend.maximumCameraPerspective, spRend.maximumCameraPerspective));

                        offset = new Point((int)Math.Round(spi * vec.X), (int)Math.Round(spi * vec.Y));
                    }
                    RenderSingleSprite(batch, spRend, tex, offset, spi*0.0001f);
                }
                
            }

            batch.End();
        }

        public void RenderSingleSprite(SpriteBatch batch, SpriteRenderer spRend, Texture2D tex, Point positionOffset, float drawOrderOffset = 0)
        {
            if (tex != null)
            {
                // get values
                Vector2 screenPos = WorldToScreenPos(spRend.sceneObject.Position);
                Vector3 scale = spRend.sceneObject.Scale;
                float ang = spRend.sceneObject.Rotation;

                // scaling
                Vector2 scaledTex = new Vector2(tex.Width * scale.X, tex.Height * scale.Y);

                Rectangle drawRect = new Rectangle(
                    new Point((int)screenPos.X+positionOffset.X, (int)screenPos.Y+positionOffset.Y),
                    new Point((int)scaledTex.X, (int)scaledTex.Y));
                batch.Draw(tex, drawRect, null, spRend.GetColor(), ang, new Vector2(tex.Width / 2, tex.Height / 2), SpriteEffects.None, spRend.drawOrder+drawOrderOffset);

            }
        }

        public void AddRenderer(SpriteRenderer renderer)
        {
            renderers.Add(renderer);
            Debug.WriteLine("Added SpriteRenderer to SceneCamera");
        }
        public void AddRenderer(StackRenderer renderer)
        {
            stackRenderers.Add(renderer);
            Debug.WriteLine("Added StackRenderer (containing " + renderer.spriteCount + " sprites) to SceneCamera");
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
