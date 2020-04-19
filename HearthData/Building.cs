using ObjectEditor.Json;

namespace HearthData
{
    public class Building : GameData
    {
        public string name;
        [FieldTag(FieldTags.Image, "Icons")]
        public string icon;
        [FieldTag(FieldTags.Image, "Buildings")]
        public string sprite;

        public int cellWidth, cellHeight;

        public override string ToString()
        {
            return name;
        }
    }
}
