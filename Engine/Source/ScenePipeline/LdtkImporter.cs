using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.ScenePipeline
{
    class LdtkImporter : JsonContentTypeReader<LdtkData>
    {
        protected override LdtkData Read(ContentReader reader, LdtkData existingInstance)
        {
            Debug.WriteLine("Loading LDTK file...");
            return base.Read(reader, existingInstance);
        }
    }
}
