using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using StarshotEngine;
using StarshotEngine.ScenePipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

public abstract class SceneObjectComponent
{
    public SceneObject sceneObject { get; private set; }

    public void InitializeValues(SceneObject owner, SceneObjectComponentData data = null)
    {
        sceneObject = owner;

        if (data != null)
        {
            // attempt to modify variables of component using the dynamic data in the SceneObjectComponentData
            foreach (KeyValuePair<string, JToken> pair in data.data)
            {
                // use reflection to try and get the public field
                FieldInfo field = this.GetType().GetField(pair.Key, BindingFlags.Public | BindingFlags.Instance);
                if (field != null && !field.IsInitOnly)
                {
                    Debug.WriteLine($"Setting field value '{pair.Key}' of '{this.GetType().Name}' component to '{pair.Value}'");

                    // cast to types based on JTokenType and write to the field
                    switch (pair.Value.Type)
                    {
                        case JTokenType.Integer:
                            field.SetValue(this, pair.Value.ToObject<int>());
                            break;
                        case JTokenType.Float:
                            field.SetValue(this, pair.Value.ToObject<float>());
                            break;
                        case JTokenType.String:
                            field.SetValue(this, pair.Value.ToObject<string>());
                            break;
                        case JTokenType.Boolean:
                            field.SetValue(this, pair.Value.ToObject<bool>());
                            break;

                        // for now, assume arrays are vectors
                        // TODO: add support for int, string and byte arrays
                        case JTokenType.Array:
                            field.SetValue(this, pair.Value.ToObject<float[]>());
                            break;
                    }
                }
                else Debug.WriteLine($"'{this.GetType().Name}' component does not have a field named '{pair.Key}'");
            }
        }

        OnCreate();
    }

    public void Update(GameTime gameTime) { OnUpdate(gameTime); }
    public void Destroy() { OnDestroy(); }

    protected virtual void OnCreate() { }
    protected virtual void OnUpdate(GameTime gameTime) { }
    protected virtual void OnDestroy() { }
}
