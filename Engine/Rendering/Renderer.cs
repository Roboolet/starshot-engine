using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine.Rendering
{
    public abstract class Renderer : SceneObjectComponent
    {
        public string filePath;
        public float[] color;

        //positive only
        public int layer;
        // float between 0.0 and 1.0
        public float drawOrder;

        protected SceneCamera camera;
        protected override void OnCreate()
        {
            SetContent(filePath);
            drawOrder = Math.Clamp(drawOrder, 0, 1);
            camera = Main.instance.currentScene.camera;
            camera.AddRenderer(this);
        }

        public abstract void DrawToSpritebatch(SpriteBatch batch);

        public void SetContent(string path)
        {
            filePath = path;
            if (!String.IsNullOrEmpty(path)) ApplyContent(path);
            else
            {
                SetContentToFallback();
            }
        }

        /// <summary>
        /// Is called after SetContent has been called
        /// </summary>
        public abstract void ApplyContent(string path="");

        public abstract void SetContentToFallback();

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
