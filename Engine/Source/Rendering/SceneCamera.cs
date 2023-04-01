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

        // draw what the camera sees to the spritebatch
        public void Render()
        {
            SpriteBatch batch = new SpriteBatch(Main.instance.GraphicsDevice);
            batch.Begin(SpriteSortMode.FrontToBack, transformMatrix:matrix);

            // render all Renderers
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].DrawToSpritebatch(batch);
            }

            batch.End();
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
