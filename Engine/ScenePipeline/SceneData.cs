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
        public Vector3 camPosition;
        public float camRotation;

        public List<SceneObjectData> objects;
    }

    [System.Serializable]
    public class SceneObjectData
    {
        public string name;
        public Vector3 position;
        public float rotation;
        public Vector3 scale;

        public List<SceneObjectComponentData> components;
        public List<SceneObjectData> children;
    }

    [System.Serializable]
    public class SceneObjectComponentData
    {
        public string type;
        public JObject data;
    }
}
