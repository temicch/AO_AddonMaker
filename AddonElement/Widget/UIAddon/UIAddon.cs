﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AddonElement
{
    [Serializable]
    public class UIAddon : File, IUIElement
    {
        public string Name { get; set; }
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
                if (bool.TryParse(value, out bool result))
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

        [XmlIgnore]
        public List<File> Widgets
        {
            get => Forms?.Select(x => x.Form?.File)?.ToList();
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
    }
}
