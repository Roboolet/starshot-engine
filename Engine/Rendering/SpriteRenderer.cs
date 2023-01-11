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

        Texture2D sprite;

        protected override void OnCreate()
        {
            SetSprite(spritePath);

            drawOrder = Math.Clamp(drawOrder, 0, 1);

            Main.instance.currentScene.camera.AddRenderer(this);
        }

        public virtual void SetSprite(string path)
        {
            spritePath = path;
            try
            {
                Texture2D tex = Main.instance.Content.Load<Texture2D>(spritePath);
                SetSprite(tex);
            }
            catch (ContentLoadException)
            {
                SetSpriteToFallback();
            }
        }

        public virtual void SetSprite(Texture2D tex)
        {
            sprite = tex;
        }

        protected void SetSpriteToFallback()
        {
            Debug.WriteLine($" !! Could not load Texture2D '{spritePath}' from Content");
            Texture2D tex = Main.instance.Content.Load<Texture2D>("placeholder");
            SetSprite(tex);
        }

        public virtual Texture2D GetSprite(int i) => sprite;
        public virtual Texture2D GetSprite() => sprite;

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
