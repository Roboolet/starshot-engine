using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace StarshotEngine.Rendering
{
    public class StackRenderer : Renderer
    {
        public bool followCamera = false;
        public bool singleSpriteMode = false;
        public int singleSpriteAmount = 1;
        // only used if perspective has been overridden, vector2
        public float[] basisVector;
        public float maximumCameraPerspective = 16;
        // vector3, multiplies distance
        public float magnitude = 1;

        List<Texture2D> sprites = new List<Texture2D>();
        public int spriteCount => sprites.Count;

        public void SetSprite(Texture2D tex)
        {
            sprites.Clear();
            sprites.Add(tex);
        }
        public void SetSprite(List<Texture2D> tex)
        {
            sprites = tex;
        }

        public override void ApplyContent(string path = "")
        {
            sprites.Clear();

            // duplicate a single sprite an amount of times
            if (singleSpriteMode)
            {
                try
                {
                    Texture2D tex = Main.instance.Content.Load<Texture2D>(path);
                    for (int i = 0; i < singleSpriteAmount; i++)
                    {
                        sprites.Add(tex);
                    }
                }
                catch (ContentLoadException)
                {
                    SetContentToFallback();
                }
            }
            // load a sequence of numbered sprites
            else
            {
                bool unbrokenSeq = true;
                if (path == null)
                {
                    unbrokenSeq = false;
                }

                // look for a sequence
                int i = 1;
                while (unbrokenSeq)
                {
                    string seq = path + i.ToString();
                    try
                    {
                        Texture2D tex = Main.instance.Content.Load<Texture2D>(seq);
                        sprites.Add(tex);

                        i++;
                    }
                    catch (ContentLoadException)
                    {
                        unbrokenSeq = false;
                        Debug.WriteLine("End of StackRenderer sequence: " + seq);
                    }
                }
            }
        }

        public override void DrawToSpritebatch(SpriteBatch batch)
        {
            if (!sceneObject.Enabled) return;

            // loop over each sprite in the stack
            for (int spi = 0; spi < spriteCount; spi++)
            {
                // get texture 
                Texture2D tex = sprites[spi];
                Point offset = Point.Zero;

                if (!followCamera)
                {
                    Vector2 vec = new Vector2(basisVector[0], basisVector[1]);
                    offset = new Point((int)Math.Round(spi * vec.X * magnitude), (int)Math.Round(spi * vec.Y * magnitude));
                }
                else
                {
                    // render based on camera position in relation to the renderer
                    float diffY = camera.Position.Y - sceneObject.Position.Y;
                    float diffX = sceneObject.Position.X - camera.Position.X;

                    Vector2 vec = new Vector2(Math.Clamp(diffX * magnitude * camera.pixelsPerUnit, -maximumCameraPerspective, maximumCameraPerspective),
                        Math.Clamp(diffY * magnitude * camera.pixelsPerUnit, -maximumCameraPerspective, maximumCameraPerspective));

                    offset = new Point((int)Math.Round(spi * vec.X), (int)Math.Round(spi * vec.Y));
                }
                RenderSingleSprite(batch, tex, offset, spi * 0.0001f);
            }
        }
        public void RenderSingleSprite(SpriteBatch batch, Texture2D tex, Point positionOffset, float drawOrderOffset = 0)
        {
            if (tex != null)
            {
                // get values
                Point screenPos = camera.WorldToScreenPos(sceneObject.Position);
                Vector3 scale = sceneObject.Scale;
                float ang = sceneObject.Rotation;

                // scaling
                Vector2 scaledTex = new Vector2(MathF.Round(tex.Width * scale.X), MathF.Round(tex.Height * scale.Y));

                Rectangle drawRect = new Rectangle(
                    new Point(screenPos.X + positionOffset.X, screenPos.Y + positionOffset.Y),
                    new Point((int)scaledTex.X, (int)scaledTex.Y));
                batch.Draw(tex, drawRect, null, GetColor(), ang, new Vector2(tex.Width / 2f, tex.Height / 2f), SpriteEffects.None, drawOrder + drawOrderOffset);
            }
        }

        public override void SetContentToFallback()
        {
            Texture2D tex = Main.instance.Content.Load<Texture2D>("placeholder");
            SetSprite(tex);
        }

    }
}
