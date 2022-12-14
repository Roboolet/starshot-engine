using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace StarshotEngine.SceneObjectComponent
{
    public class ComponentLibrary
    {

        static Dictionary<string, ISceneObjectComponent> dict;
        public static void Initialize()
        {
            Debug.WriteLine("Initializing ComponentLibrary...");
            dict = new Dictionary<string, ISceneObjectComponent>();

            // get all types in the current assembly
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for(int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                // check if type is an implementation of ISceneObjectComponent
                if(type.IsAssignableFrom(typeof(ISceneObjectComponent)) && !type.IsInterface)
                {
                    dict.Add(type.Name, (ISceneObjectComponent)type);
                    Debug.WriteLine("Added component to library: " + type.Name);
                }
                Debug.WriteLine("Skipped component: " + type.Name);
            }
        }

        public static ISceneObjectComponent GetComponentType(string className)
        {
            if(dict != null)
            {
                if(dict.TryGetValue(className, out ISceneObjectComponent comp))
                {
                    return comp;
                }

                Debug.WriteLine("ComponentLibrary does not contain an entry for "+ className);
                return null;
            }

            Debug.WriteLine("ComponentLibrary is null");
            return null;
        }
    }
}
