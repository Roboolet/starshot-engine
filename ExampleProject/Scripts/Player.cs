using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExampleProject.Scripts
{
    class Player : SceneObjectComponent
    {
        const float speed = 16f;
        const float maxSpeed = 12f;
        float vel;

        protected override void OnCreate()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            // change velocity based on input
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.S)) vel = vel - gameTime.GetElapsedSeconds() * speed;
            if (state.IsKeyDown(Keys.W)) vel = vel + gameTime.GetElapsedSeconds() * speed;
            vel = Math.Clamp(vel, -maxSpeed, maxSpeed);

            // change angle
            if (state.IsKeyDown(Keys.A)) sceneObject.Rotation += 0.002f*vel;
            if (state.IsKeyDown(Keys.D)) sceneObject.Rotation += -0.002f*vel;

            // move player
            sceneObject.Position += Vector3.Normalize(sceneObject.matrix.Up) * vel * gameTime.GetElapsedSeconds();

            // friction
            vel = vel - vel * gameTime.GetElapsedSeconds();
        }
    }
}
