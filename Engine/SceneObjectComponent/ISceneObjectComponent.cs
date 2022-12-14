using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public interface ISceneObjectComponent
{
    public void OnStart() { }
    public void OnUpdate(GameTime gameTime) { }
    public void OnDestroy() { }
}
