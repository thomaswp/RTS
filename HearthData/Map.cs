using ObjectEditor.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthData
{
    public class Map : GameData
    {
        public readonly Reference<MapTile> defaultTile = new Reference<MapTile>();
        public int tileWidth, tileHeight;
    }
}
