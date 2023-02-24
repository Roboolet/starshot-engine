using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.Rendering
{

    public class ScreenspaceScaler : SceneObjectComponent
    {
        SceneCamera camera;

        public float[] targetScale;
        public float[] anchor = new float[2];

        protected override void OnStart()
        {
            camera = Main.instance.currentScene.camera;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            sceneObject.Position = camera.ScreenToWorldPos(anchor[0], anchor[1]);
            // Debug.WriteLine(sceneObject.Position);
        }
    }
}
