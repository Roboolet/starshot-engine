using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StarshotEngine;
using StarshotEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleProject
{
    class SpinComponent : SceneObjectComponent
    {
        float rot = 1;

        protected override void OnUpdate(GameTime gameTime)
        {
            sceneObject.Rotation = rot * (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A)) rot += (float)gameTime.ElapsedGameTime.TotalSeconds*10;
            if (state.IsKeyDown(Keys.D)) rot -= (float)gameTime.ElapsedGameTime.TotalSeconds*10;
        }
    }
}
