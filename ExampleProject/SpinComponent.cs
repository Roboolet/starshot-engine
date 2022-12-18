using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleProject
{
    class SpinComponent : SceneObjectComponent
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            sceneObject.Rotation += 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
