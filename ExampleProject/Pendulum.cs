using Microsoft.Xna.Framework;
using StarshotEngine;
using StarshotEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleProject
{
    class Pendulum : SceneObjectComponent
    {
        protected override void OnCreate()
        {
            SceneObject iterParent = sceneObject;
            for(int i = 0; i < 10; i++)
            {
                iterParent = new SceneObject(iterParent);
                iterParent.AddComponent<SpriteRenderer>();

                iterParent.Position = new Vector3(0, 1, 0);

                iterParent.AddComponent<SpinComponent>().speed = 1f;

                if(i == 9)
                {
                    iterParent.AddComponent<Drawer>();
                }

            }
        }
    }

    class Drawer : SceneObjectComponent
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            SceneObject sob = new SceneObject(null);
            sob.AddComponent<SpriteRenderer>();
            sob.Position = sceneObject.Position;
            sob.Scale = Vector3.One * 0.2f;
        }
    }
}
