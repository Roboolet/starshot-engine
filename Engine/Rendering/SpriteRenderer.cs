using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.Rendering
{
    public class SpriteRenderer : SceneObjectComponent
    {
        public string spritePath;
        public float[] color;

        // set between 0 to 1 range
        public float drawOrder;

        public Texture2D sprite { get; private set; }

        protected override void OnCreate()
        {
            Debug.WriteLine("Created SpriteRenderer :3");
            SetSprite(spritePath);

            drawOrder = Math.Clamp(drawOrder, 0, 1);
        }

        public void SetSprite(string path)
        {
            spritePath = path;
            try
            {
                Texture2D tex = Main.instance.Content.Load<Texture2D>(spritePath);
                SetSprite(tex);
            }
            catch (ContentLoadException)
            {
                Debug.WriteLine($" !! Could not load Texture2D '{path}' from Content");
            }
        }
        public void SetSprite(Texture2D tex)
        {
            sprite = tex;
        }

        public Color GetColor()
        {
            return new Color(color[0], color[1], color[2]);
        }
    }
}
