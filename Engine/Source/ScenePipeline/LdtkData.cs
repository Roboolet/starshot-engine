using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine.ScenePipeline
{
    public class LdtkData
    {
        // contains all levels and their data
        public LdtkLevel[] levels;
        public LdtkDefs[] defs;

        // version of the LDTk json
        public string jsonVersion;
        public string iid;

        public WorldLayout worldLayout;
    }

    public enum WorldLayout { Null, Free, GridVania, LinearHorizontal, LinearVertical}

    public class LdtkLevel
    {   // unique instance identifier
        public string iid;
        // user specified name
        public string identifier;
        public LdtkLevelNeighbour[] __neighbours;
    }

    public class LdtkLevelNeighbour
    {
        // n e s w
        public string dir;
        public string iid;
    }
    public class LdtkLayerInstance
    {
        // layer type : {IntGrid, Entities, Tiles, AutoLayer}
        public string __type;

        public int __cHei, __cWid, __gridSize;
        public string __identifier;
        public float __opacity;
        public int __pxTotalOffsetX, __pxTotalOffsetY;
        public int __tilesetDefUid;

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

    public class LdtkEntityInstance
    {
        // coordinates; [x,y] [x,y] [x,y] ....
        public int[] __grid;
        public string __identifier;

        // unique identifier
        public int iid;
        public int width, height;

        public string[] __tags;
        public LdtkTilesetRect __tile;
        public LdtkFieldInstance[] fieldInstances;
    }

    public class LdtkFieldInstance
    {
        public string __identifier;
        public LdtkTilesetRect __tile;

        // see https://ldtk.io/docs/game-dev/json-overview/entity-fields/ for field types
        public string __type;
        public JObject __value;

    }

    public class LdtkDefs
    {
        public LdtkDefTileset[] tilesets;
    }

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

    public class LdtkTilesetRect {public int w, h, x, y, tilesetUid;}

}
