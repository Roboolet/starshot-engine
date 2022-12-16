using Microsoft.Xna.Framework;
using StarshotEngine;
using System;
using System.Collections.Generic;
using System.Text;

public abstract class SceneObjectComponent
{

    public virtual void OnStart() { }
    public virtual void OnUpdate(GameTime gameTime) { }
    public virtual void OnDestroy() { }
}
