using ObjectEditor.Json;

namespace HearthData
{
    public class ResourceSource : GameData
    {
        [FieldTag(FieldTags.Image, "objects")]
        public string sprite;
        public float rate = 1;
        public float limit = 1000;

        public override string ToString()
        {
            return name;
        }
    }
}
