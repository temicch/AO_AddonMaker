using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AddonElement
{
    [Serializable]
    public class UIAddon : File, IUIElement
    {
        public UIAddon()
        {
            AutoStart = true;
        }

        public href localizedNameFileRef { get; set; }
        public href localizedDescFileRef { get; set; }

        [XmlIgnore] 
        public bool AutoStart { get; set; }

        [XmlElement("AutoStart")]
        public string _AutoStart
        {
            get => AutoStart.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    AutoStart = result;
            }
        }

        [XmlArrayItem("Item")] 
        public List<href> addonGroups { get; set; }

        [XmlArrayItem("Item")] 
        public List<href> ScriptFileRefs { get; set; }

        public string MainFormId { get; set; }

        [XmlArrayItem("Item")] 
        public List<FormItem> Forms { get; set; }

        public href visObjects { get; set; }
        public href aliasVisObjects { get; set; }
        public href texts { get; set; }

        [XmlArrayItem("Item")] 
        private List<TextsItem> textsGroups { get; set; }

        public href textures { get; set; }

        [XmlArrayItem("Item")] 
        private List<TexturesItem> texturesGroups { get; set; }

        public href sounds { get; set; }

        [XmlArrayItem("Item")] 
        private List<SoundsItem> soundsGroups { get; set; }

        public href decalObjects { get; set; }
        public string Name { get; set; }

        [XmlIgnore] 
        public List<File> Widgets => Forms?.Select(x => x.Form?.File)?.ToList();

        [XmlIgnore] 
        public WidgetPlacementXY Placement { get; set; }

        [XmlIgnore] 
        public bool Visible { get; set; }

        [XmlIgnore] 
        public bool Enabled { get; set; }
    }
}