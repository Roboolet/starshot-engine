using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Linq;

public class ComponentClassLibrary
{

    static Dictionary<string, Type> dict;
    public static void Initialize()
    {
        Debug.WriteLine("Initializing ComponentLibrary...");
        dict = new Dictionary<string, Type>();

        // get all types in the current assembly
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        for (int i = 0; i < types.Length; i++)
        {
            Type type = types[i];
            // check if type is an implementation of SceneObjectComponent
            if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Renderer)))
            {
                dict.Add(type.Name, type);
                Debug.WriteLine("Added component to library: " + type.Name);
            }
            Debug.WriteLine("Skipped component: " + type.Name);
        }
    }

    public static Type GetComponentType(string className)
    {
        if (dict != null)
        {
            if (dict.TryGetValue(className, out Type compType))
            {
                return compType;
            }

            Debug.WriteLine("ComponentLibrary does not contain an entry for " + className);
            return null;
        }

        Debug.WriteLine("ComponentLibrary is null");
        return null;
    }
}
