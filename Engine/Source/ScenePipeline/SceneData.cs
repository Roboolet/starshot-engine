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
        public string name;
        public float[] camPosition;
        public float camRotation;

        public SceneObjectData[] objects;
    }

    [System.Serializable]
    public class SceneObjectData
    {
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
