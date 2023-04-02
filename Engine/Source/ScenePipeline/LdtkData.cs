using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine.ScenePipeline
{
    [System.Serializable]
    public class LdtkData
    {
        // contains all levels and their data
        public LdtkLevel[] levels;
        public LdtkDefs defs;

        // version of the LDTk json
        public string jsonVersion;
        public string iid;

        public WorldLayout worldLayout;
    }

    public enum WorldLayout { Null, Free, GridVania, LinearHorizontal, LinearVertical}

    [System.Serializable]
    public class LdtkLevel
    {   // unique instance identifier
        public string iid;
        // user specified name
        public string identifier;
        public LdtkLevelNeighbour[] __neighbours;
        public LdtkLayerInstance[] layerInstances;
        public LdtkFieldInstance[] fieldInstances;
    }

    [System.Serializable]
    public class LdtkLevelNeighbour
    {
        // n e s w
        public string dir;
        public string iid;
    }

    [System.Serializable]
    public class LdtkLayerInstance
    {
        // layer type : {IntGrid, Entities, Tiles, AutoLayer}
        public string __type;

        public int __cHei, __cWid, __gridSize;
        public string __identifier;
        public float __opacity;
        public int __pxTotalOffsetX, __pxTotalOffsetY;
        public Nullable<int> __tilesetDefUid;

        // path to source image relative to .ldtk file
        public string __tilesetRelPath;

        // layer specific things ---------------
        // only IntGrids:
        public int[] intGridCsv;

        // only Tile layers:
        public LdtkTileInstance[] gridTiles;

        // only AutoLayers:
        public LdtkTileInstance[] autoLayerTiles;

        // only Entity layers:
        public LdtkEntityInstance[] entityInstances;

    }

    [System.Serializable]
    public class LdtkTileInstance
    {
        // see https://ldtk.io/json/#ldtk-Tile
        public int f;

        // coordinates; [x,y] [x,y] [x,y] ....
        public int[] px;
        public int[] src;

        // tile id in tileset
        public int t;
    }

    [System.Serializable]
    public class LdtkEntityInstance
    {
        // coordinates; [x,y] [x,y] [x,y] ....
        public int[] __grid;
        public string __identifier;

        // unique identifier
        public string iid;
        public int width, height;

        public string[] __tags;
        public LdtkTilesetRect __tile;
        public LdtkFieldInstance[] fieldInstances;
    }

    [System.Serializable]
    public class LdtkFieldInstance
    {
        public string __identifier;
        public LdtkTilesetRect __tile;

        // see https://ldtk.io/docs/game-dev/json-overview/entity-fields/ for field types
        public string __type;
        public JObject __value;

    }

    [System.Serializable]
    public class LdtkDefs
    {
        public LdtkDefTileset[] tilesets;
    }

    [System.Serializable]
    public class LdtkDefTileset
    {
        public int uid;
        public int __cHei, __cWid;
        public string identifier;
        public int spacing;
        public string[] tags;

        // path to source image relative to .ldtk file
        public string relPath;
    }

    [System.Serializable]
    public class LdtkTilesetRect {public int w, h, x, y, tilesetUid;}

}
