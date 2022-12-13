
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace StarshotEngine.ScenePipeline
{
    class SceneImporter : JsonContentTypeReader<SceneData>
    {
    }
}
