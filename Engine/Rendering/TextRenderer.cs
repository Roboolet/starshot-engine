using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.Rendering
{
    public class TextRenderer : Renderer
    {
        public string fontPath;
        public string content;
        public SpriteFont font { get; private set; }

        public float[] color;
        public float drawOrder;
        public float fontScale = 1;
        public string alignment = "center";

        public override void ApplyContent(string path = "")
        {
            fontPath = path;
            try
            {
                SpriteFont fn = Main.instance.Content.Load<SpriteFont>(path);
                Debug.WriteLine("Successfully loaded SpriteFont: " + path);
                SetFont(fn);
            }
            catch (ContentLoadException)
            {
                // use fallback font
                Debug.WriteLine(" !! Could not load font at: " + path);
                SetContentToFallback();
            }
            catch (InvalidCastException)
            {
                Debug.WriteLine(" !! Could not cast file to font at: " + path);
                SetContentToFallback();
            }
        }

        public void SetFont(SpriteFont _font)
        {
            font = _font;
        }

        public override void SetContentToFallback()
        {
            SpriteFont f = Main.instance.Content.Load<SpriteFont>("roboto");
            SetFont(f);
        }

        public override void DrawToSpritebatch(SpriteBatch batch)
        {
            if (font != null)
            {
                Vector2 screenPos = camera.WorldToScreenPos(sceneObject.Position).ToVector2() - GetAlignmentVector();
                batch.DrawString(font, content, screenPos, GetColor(), sceneObject.Rotation, new Vector2(0.5f, 0.5f), fontScale, SpriteEffects.None, drawOrder);
            }
        }

        Vector2 GetAlignmentVector()
        {
            switch (alignment)
            {
                default:
                    return font.MeasureString(content) / 2;
                case "left":
                    return Vector2.Zero;
                case "right":
                    return font.MeasureString(content);
            }
        }
    }
}
