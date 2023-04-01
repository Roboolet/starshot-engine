using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using StarshotEngine;
using StarshotEngine.ScenePipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

public abstract class Renderer
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
                    WriteToField(field, pair.Value);
                }
                else
                {
                    // find property instead
                    PropertyInfo property = this.GetType().GetProperty(pair.Key, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && property.CanWrite)
                    {
                        Debug.WriteLine($"Setting property value '{pair.Key}' of '{this.GetType().Name}' component to '{pair.Value}'");
                        WriteToProperty(property, pair.Value);
                    }
                    else Debug.WriteLine($"'{this.GetType().Name}' component does not have a field or property named '{pair.Key}'");
                }
            }
        }

        OnCreate();
    }

    void WriteToProperty(PropertyInfo info, JToken value)
    {
        // cast to types based on JTokenType and write to the field
        switch (value.Type)
        {
            case JTokenType.Integer:
                info.SetValue(this, value.ToObject<int>());
                break;
            case JTokenType.Float:
                info.SetValue(this, value.ToObject<float>());
                break;
            case JTokenType.String:
                info.SetValue(this, value.ToObject<string>());
                break;
            case JTokenType.Boolean:
                info.SetValue(this, value.ToObject<bool>());
                break;

            // for now, assume arrays are vectors
            // TODO: add support for any array type
            case JTokenType.Array:
                info.SetValue(this, value.ToObject<float[]>());
                break;
        }
    }

    void WriteToField(FieldInfo info, JToken value)
    {
        // cast to types based on JTokenType and write to the field
        switch (value.Type)
        {
            case JTokenType.Integer:
                info.SetValue(this, value.ToObject<int>());
                break;
            case JTokenType.Float:
                info.SetValue(this, value.ToObject<float>());
                break;
            case JTokenType.String:
                info.SetValue(this, value.ToObject<string>());
                break;
            case JTokenType.Boolean:
                info.SetValue(this, value.ToObject<bool>());
                break;

            // for now, assume arrays are vectors
            // TODO: add support for any array type
            case JTokenType.Array:
                info.SetValue(this, value.ToObject<float[]>());
                break;
        }
    }

    public void Start() { OnStart(); }
    public void Update(GameTime gameTime) { OnUpdate(gameTime); }
    public void Destroy() { OnDestroy(); }

    /// <summary>
    /// Called after the scene has finished populating, or immediately after spawned. Called after OnCreate
    /// </summary>
    protected virtual void OnStart() { }
    protected virtual void OnCreate() { }
    protected virtual void OnUpdate(GameTime gameTime) { }
    protected virtual void OnDestroy() { }
}
