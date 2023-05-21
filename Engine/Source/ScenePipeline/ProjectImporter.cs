
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace StarshotEngine.ScenePipeline
{
    class ProjectImporter : JsonContentTypeReader<ProjectData>
    {
        protected override ProjectData Read(ContentReader reader, ProjectData existingInstance)
        {
            return base.Read(reader, existingInstance);
        }
    }
}
