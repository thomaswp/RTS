using ObjectEditor.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthData
{
    public class Unit : GameData
    {
        [FieldTag(FieldTags.Image, "Units")]
        public string sprite;

        public override string ToString()
        {
            return name;
        }
    }
}
