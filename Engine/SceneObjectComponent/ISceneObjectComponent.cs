using System;
using System.Collections.Generic;
using System.Text;

public interface ISceneObjectComponent
{
    public void OnStart() { }
    public void OnUpdate() { }
    public void OnDestroy() { }
}
