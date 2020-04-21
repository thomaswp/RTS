using System;
using ObjectEditor.Json;
using System.Linq;
using System.Collections.Generic;

namespace HearthData
{
    public class HearthGame : GameData, IScriptable
    {
        public readonly Reference<Map> startingMap = new Reference<Map>();

        public readonly List<Resource> resources = new List<Resource>();
        public readonly List<ResourceSource> resourceSources = new List<ResourceSource>();
        public readonly List<Building> buildings = new List<Building>();
        public readonly List<Unit> units = new List<Unit>();
        public readonly List<Map> maps = new List<Map>();
        public readonly List<MapTile> mapTiles = new List<MapTile>();

    }
}
