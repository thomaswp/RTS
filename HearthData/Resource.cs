using ObjectEditor.Json;

namespace HearthData
{
    public class Resource : GameData
    {
        public string name;
        [FieldTag(FieldTags.Image, "icon")]
        public string icon;

        public override string ToString()
        {
            return name;
        }
    }
}
