using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StarshotEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExampleProject.Scripts
{
    public class Collectable : SceneObjectComponent
    {
        SceneObject plr;
        public float grabDist;

        protected override void OnStart()
        {
            plr = SceneObject.Find(Main.instance.currentScene.root, "Player");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            // since there is currently no collision system, manually do a distance check to the player
            if(plr != null)
            {
                float dist = Vector3.Distance(plr.Position, sceneObject.Position);

                if (grabDist > dist)
                {
                    Grab();
                }
            }

            // animation
            sceneObject.Position = sceneObject.Position + new Vector3(0, MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds*1.5f), 0) * gameTime.GetElapsedSeconds(); ;
        }

        void Grab()
        {
            Random rnd = new Random();
            Vector3 r = new Vector3(rnd.Next(-5, 5), rnd.Next(-5, 5), 0);

            Debug.WriteLine("Old position " + sceneObject.Position);
            sceneObject.Position = r;

            Debug.WriteLine("Rand = " + r);
            Debug.WriteLine("New position = " + sceneObject.Position);
        }
    }
}
