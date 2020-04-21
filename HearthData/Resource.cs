using ObjectEditor.Json;

namespace HearthData
{
    public class Resource : GameData
    {
        [FieldTag(FieldTags.Image, "icon")]
        public string icon;
        
    }
}
