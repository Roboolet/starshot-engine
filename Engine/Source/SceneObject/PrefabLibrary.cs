using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StarshotEngine.ScenePipeline;

namespace StarshotEngine
{
    public class PrefabLibrary
    {
        static Dictionary<string, SceneObjectData> prefabs = new Dictionary<string, SceneObjectData>();

        public static bool PrefabExists(string prefabID) => prefabs.ContainsKey(prefabID);

        public static SceneObject Instantiate(string prefabID, SceneObject parent = null)
        {
            if (prefabs.TryGetValue(prefabID, out SceneObjectData data))
            {
                SceneObject prefabRoot = new SceneObject(parent, data);
                prefabRoot.name = data.prefabID + " Instance";
                return prefabRoot;
            }
            else
            {
                Debug.WriteLine(" !!! No prefab found with ID: " + prefabID);
                return null;
            }
        }

        public static bool TryGetPrefab(string prefabID, out SceneObjectData prefab)
        {
            if (prefabs.TryGetValue(prefabID, out SceneObjectData data))
            {
                prefab = data;
                return true;
            }
            else
            {
                Debug.WriteLine(" !!! No prefab found with ID: " + prefabID);
                prefab = null;
                return false;
            }
        }

        public static SceneObjectData CombineObjects(SceneObjectData original, string prefabID)
        {
            if (TryGetPrefab(prefabID, out SceneObjectData pf))
            {
                SceneObjectData copy = original;
                copy.children = copy.children.Concat(pf.children).ToArray();
                copy.components = copy.components.Concat(pf.components).ToArray();
                return copy;
            }
            return original;
        }

        public static void AddPrefab(SceneObjectData data)
        {
            if (!prefabs.TryAdd(data.prefabID, data))
            {
                Debug.WriteLine(" !!! Multiple prefabs with same ID: " + data.prefabID);
            }
        }
    }
}
