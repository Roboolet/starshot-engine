
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace StarshotEngine.ScenePipeline
{
    class PrefabImporter : JsonContentTypeReader<SceneObjectData>
    {
        protected override SceneObjectData Read(ContentReader reader, SceneObjectData existingInstance)
        {
            SceneObjectData data = base.Read(reader, existingInstance);
            data.prefabID = reader.AssetName;
            Debug.WriteLine("Created prefab with ID " + data.prefabID);
            return data;
        }
    }
}
