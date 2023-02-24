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
        public float pixelsPerUnit = 24;

        List<Renderer> renderers = new List<Renderer>();
        List<StackRenderer> stackRenderers = new List<StackRenderer>();
        List<TextRenderer> textRenderers = new List<TextRenderer>();

        // draw what the camera sees to the spritebatch
        public void Render()
        {
            SpriteBatch batch = new SpriteBatch(Main.instance.GraphicsDevice);
            batch.Begin(SpriteSortMode.FrontToBack, transformMatrix:matrix);

            // render all SpriteRenderers
            /*for(int i = 0; i < renderers.Count; i++)
            {
                SpriteRenderer spRend = renderers[i];

                // get texture 
                Texture2D tex = spRend.GetSprite();
                RenderSingleSprite(batch, spRend, tex, Point.Zero);
            }*/

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

                        Vector2 vec = new Vector2(Math.Clamp(diffX * spRend.magnitude * pixelsPerUnit, -spRend.maximumCameraPerspective, spRend.maximumCameraPerspective),
                            Math.Clamp(diffY * spRend.magnitude * pixelsPerUnit, -spRend.maximumCameraPerspective, spRend.maximumCameraPerspective));

                        offset = new Point((int)Math.Round(spi * vec.X), (int)Math.Round(spi * vec.Y));
                    }
                    RenderSingleSprite(batch, spRend, tex, offset, spi*0.0001f);
                }
                
            }
            // render all Renderers
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].DrawToSpritebatch(batch);
            }

            batch.End();
        }

        public void RenderSingleSprite(SpriteBatch batch, SpriteRenderer spRend, Texture2D tex, Point positionOffset, float drawOrderOffset = 0)
        {
            if (tex != null)
            {
                // get values
                Point screenPos = WorldToScreenPos(spRend.sceneObject.Position);
                Vector3 scale = spRend.sceneObject.Scale;
                float ang = spRend.sceneObject.Rotation;

                // scaling
                Vector2 scaledTex = new Vector2(MathF.Round(tex.Width * scale.X), MathF.Round(tex.Height * scale.Y));

                Rectangle drawRect = new Rectangle(
                    new Point(screenPos.X+positionOffset.X, screenPos.Y+positionOffset.Y),
                    new Point((int)scaledTex.X, (int)scaledTex.Y));
                batch.Draw(tex, drawRect, null, spRend.GetColor(), ang, new Vector2(tex.Width / 2f, tex.Height / 2f), SpriteEffects.None, spRend.drawOrder+drawOrderOffset);
            }
        }
        public void AddRenderer(StackRenderer renderer)
        {
            stackRenderers.Add(renderer);
            Debug.WriteLine("Added StackRenderer (containing " + renderer.spriteCount + " sprites) to SceneCamera");
        }
        public void AddRenderer(TextRenderer renderer)
        {
            textRenderers.Add(renderer);
            Debug.WriteLine("Added TextRenderer (" + renderer.content + ") to SceneCamera");
        }
        public void AddRenderer(Renderer renderer)
        {
            renderers.Add(renderer);
            Debug.WriteLine("Added Renderer (" + renderer.sceneObject.name + ")to SceneCamera");
        }

        public Point WorldToScreenPos(Vector3 vec) => WorldToScreenPos(vec.X, vec.Y);
        public Point WorldToScreenPos(float x, float y)
        {
            Viewport vp = Main.instance.GraphicsDevice.Viewport;
            // get coord, subtract camera, and center in screen.
            // additionally, invert the Y axis
            Point newVec = new Point(
                (int)MathF.Round(x * pixelsPerUnit - Position.X * pixelsPerUnit + vp.Width / 2f - x),
                (int)MathF.Round(-y * pixelsPerUnit + Position.Y * pixelsPerUnit + vp.Height / 2f - y));
            return newVec;
        }

        public Vector3 ScreenToWorldPos(float u, float v)
        {
            Viewport vp = Main.instance.GraphicsDevice.Viewport;
            // multiply u and v by the width and height, add the result to the position of the camera
            Vector3 newVec = new Vector3(
                u * (vp.Width/pixelsPerUnit) - (vp.Width / pixelsPerUnit /2f) + Position.X,
                -v * (vp.Height/pixelsPerUnit) + (vp.Height / pixelsPerUnit /2f) + Position.Y,
                0);
            return newVec;
        }
    }
}
