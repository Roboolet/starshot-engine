using Microsoft.Xna.Framework;
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

        protected override void OnCreate()
        {
            plr = SceneObject.Find(null, "Gas");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            // since there is currently no collision system, manually do a distance check to the player
            if(Vector3.Distance(plr.Position, sceneObject.Position) <= grabDist)
            {
                // grab
                Debug.WriteLine("Grab!");
            }
        }
    }
}
