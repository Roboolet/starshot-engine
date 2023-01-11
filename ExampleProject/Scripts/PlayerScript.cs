using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using StarshotEngine;
using StarshotEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExampleProject.Scripts
{
    class PlayerScript : SceneObjectComponent
    {
        public float cameraFollowSpeed;
        public float speed;
        public float maxSpeed;

        float vel;
        float rvel;
        SceneCamera cam;

        protected override void OnCreate()
        {
            cam = Main.instance.currentScene.camera;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            // change velocity based on input
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.S)) vel = vel - gameTime.GetElapsedSeconds() * speed;
            if (state.IsKeyDown(Keys.W)) vel = vel + gameTime.GetElapsedSeconds() * speed;
            vel = Math.Clamp(vel, -maxSpeed, maxSpeed);

            // change angle
            if (state.IsKeyDown(Keys.A))
            {
                rvel = Math.Max(rvel, 0);
                rvel += 0.0002f;
            }
            else if (state.IsKeyDown(Keys.D))
            {
                rvel = Math.Min(rvel, 0);
                rvel += -0.0002f;
            }
            else rvel *= 0.8f;

            rvel = Math.Clamp(rvel, -0.002f, 0.002f);
            sceneObject.Rotation += rvel * vel;

            // move player
            sceneObject.Position += Vector3.Normalize(sceneObject.matrix.Up) * vel * gameTime.GetElapsedSeconds();

            // friction
            vel = vel - vel * gameTime.GetElapsedSeconds();

            // camera follow
            cam.Position += (sceneObject.Position - cam.Position) * gameTime.GetElapsedSeconds()*cameraFollowSpeed;
        }
    }
}
