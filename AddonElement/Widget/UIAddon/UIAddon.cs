using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AO_AddonMaker
{
    [Serializable]
    public class UIAddon : AddonFile, IUIElement
    {
        public string Name { get; set; }
        public href localizedNameFileRef { get; set; }
        public href localizedDescFileRef { get; set; }
        public bool AutoStart { get; set; }

        [XmlArrayItem("Item")]
        public List<href> addonGroups { get; set; }

        [XmlArrayItem("Item")]
        public List<href> ScriptFileRefs { get; set; }

        public string MainFormId { get; set; }

        [XmlArrayItem("Item")]
        public List<FormItem> Forms { get; set; }

        [XmlIgnore]
        public List<AddonFile> Widgets
        {
            get => Forms.Select(x => x.Form.File).ToList();
        }

        public href visObjects { get; set; }
        public href aliasVisObjects { get; set; }
        public href texts { get; set; }

        [XmlArrayItem("Item")]
        List<TextsItem> textsGroups { get; set; }

        public href textures { get; set; }

        [XmlArrayItem("Item")]
        List<TexturesItem> texturesGroups { get; set; }

        public href sounds { get; set; }

        [XmlArrayItem("Item")]
        List<SoundsItem> soundsGroups { get; set; }

        public href decalObjects { get; set; }

        [XmlIgnore]
        public WidgetPlacementXY Placement { get; set; }
        [XmlIgnore]
        public bool Visible { get; set; }
        [XmlIgnore]
        public bool Enabled { get; set; }

        public UIAddon()
        {
            AutoStart = true;
        }

        public IEnumerable<AddonFile> GetChildren() => Forms.Select(x => x.Form.File);
    }

    public class FormItem
    {
        public string Id { get; set; }
        public href Form;
    }

    public class TextsItem
    {
        public string groupName { get; set; }
        public href texts;
    }

    public class TexturesItem
    {
        public string groupName { get; set; }
        public href textures;
    }

    public class SoundsItem
    {
        public string groupName { get; set; }
        public href sounds;
    }
}
