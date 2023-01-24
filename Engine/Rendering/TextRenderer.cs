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

        protected override void OnCreate()
        {
            SetFont(fontPath);
        }

        public void SetFont(string path)
        {
            fontPath = path;
            try
            {
                Texture2D tex = Main.instance.Content.Load<Texture2D>(path);
                SetFont(path);
            }
            catch (ContentLoadException)
            {
                // use fallback font
                Debug.WriteLine("!! Could not load font at: " + path);
            }
        }

        public void SetFont(SpriteFont _font)
        {
            font = _font;
        }
    }
}
