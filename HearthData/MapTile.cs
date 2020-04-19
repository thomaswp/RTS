using ObjectEditor.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthData
{
    public class MapTile : GameData
    {
        public string name;

        [FieldTag(FieldTags.Image, "Tiles")]
        public string sprite;
    }
}
