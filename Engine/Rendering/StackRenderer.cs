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
    public class StackRenderer : SpriteRenderer
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

        protected override void OnCreate()
        {
            SetSprite(spritePath);
            drawOrder = Math.Clamp(drawOrder, 0, 1);
            Main.instance.currentScene.camera.AddRenderer(this);
        }

        public override Texture2D GetSprite() => GetSprite(0);

        public override Texture2D GetSprite(int id)
        {
            if (id < sprites.Count) return sprites[id];
            else return null;
        }

        /// <summary>
        /// Gets all sprites that start with this path, ordered numerically
        /// Sprites must be numbered starting from 1, the path should not contain the numbering
        /// Only .PNG format is supported
        /// </summary>
        /// <param name="path"></param>
        public override void SetSprite(string path)
        {
            spritePath = path;
            sprites.Clear();

            // duplicate a single sprite an amount of times
            if (singleSpriteMode)
            {
                try
                {
                    Texture2D tex = Main.instance.Content.Load<Texture2D>(spritePath);
                    for(int i = 0; i < singleSpriteAmount; i++)
                    {
                        sprites.Add(tex);
                    }
                }
                catch (ContentLoadException)
                {
                    SetSpriteToFallback();
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
                    string seq = spritePath + i.ToString();
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

        public override void SetSprite(Texture2D tex)
        {
            sprites.Clear();
            sprites.Add(tex);
        }

    }
}
