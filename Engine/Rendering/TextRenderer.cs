using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.Rendering
{
    public class TextRenderer : SceneObjectComponent
    {
        public string fontPath;
        public string content;
        public SpriteFont font { get; private set; }

        public float[] color;
        public float drawOrder;
        public float fontScale = 1;

        protected override void OnCreate()
        {
            SetFont(fontPath);
            Main.instance.currentScene.camera.AddRenderer(this);
        }

        public void SetFont(string path)
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
                SetFontToFallback();
            }
            catch(InvalidCastException)
            {
                Debug.WriteLine(" !! Could not cast file to font at: " + path);
                SetFontToFallback();
            }
        }

        public void SetFont(SpriteFont _font)
        {
            font = _font;
        }
        protected void SetFontToFallback()
        {
            SpriteFont f = Main.instance.Content.Load<SpriteFont>("roboto");
            SetFont(f);
        }


        public Color GetColor()
        {
            if (color != null)
            {
                return new Color(color[0], color[1], color[2]);
            }
            else return Color.White;
        }
    }
}
