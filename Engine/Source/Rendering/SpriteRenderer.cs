using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.Rendering
{
    public class SpriteRenderer : Renderer
    {
        public string spritePath;
        public float[] color;

        // set between 0 to 1 range
        public float drawOrder;

        Texture2D sprite;

        public virtual void SetSprite(Texture2D tex)
        {
            sprite = tex;
        }

        public Color GetColor()
        {
            if (color != null)
            {
                return new Color(color[0], color[1], color[2]);
            }
            else return Color.White;
        }

        public override void DrawToSpritebatch(SpriteBatch batch)
        {
            if (!sceneObject.Enabled) return;
            if (sprite != null)
            {
                // get values
                Point screenPos = camera.WorldToScreenPos(sceneObject.Position);
                Vector3 scale = sceneObject.Scale;
                float ang = sceneObject.Rotation;

                // scaling
                Vector2 scaledTex = new Vector2(MathF.Round(sprite.Width * scale.X), MathF.Round(sprite.Height * scale.Y));

                Rectangle drawRect = new Rectangle(
                    new Point(screenPos.X, screenPos.Y),
                    new Point((int)scaledTex.X, (int)scaledTex.Y));
                batch.Draw(sprite, drawRect, null, GetColor(), ang, new Vector2(sprite.Width / 2f, sprite.Height / 2f), SpriteEffects.None, drawOrder);
            }
        }

        public override void ApplyContent(string path = "")
        {
            spritePath = path;
            try
            {
                Texture2D tex = Main.instance.Content.Load<Texture2D>(spritePath);
                SetSprite(tex);
            }
            catch (ContentLoadException)
            {
                SetContentToFallback();
            }
        }

        public override void SetContentToFallback()
        {
            Debug.WriteLine($" !! Could not load Texture2D '{spritePath}' from Content");
            Texture2D tex = Main.instance.Content.Load<Texture2D>("placeholder");
            SetSprite(tex);
        }
    }
}
