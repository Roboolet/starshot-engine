using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine.ScenePipeline
{
    [System.Serializable]
    public class SceneData
    {
        public string ldtk;
        public string name;
        public float[] camPosition;
        public float camRotation;

        public SceneObjectData[] objects;
    }

    [System.Serializable]
    public class SceneObjectData
    {
        // if prefabID is not empty, first load the original prefab, and then modify it based on the other parameters
        // this includes adding components and children to the original prefab
        // PRS are replaced instead of added
        public string prefabID;

        public string name;
        public float[] position;
        public float rotation;
        public float[] scale;

        public SceneObjectComponentData[] components;
        public SceneObjectData[] children;
    }

    [System.Serializable]
    public class SceneObjectComponentData
    {
        public string type;
        public JObject data;
    }
}
