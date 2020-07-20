using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    [Serializable]
    public class UIAddon : File, IUIElement
    {
        public UIAddon()
        {
            AutoStart = true;
        }

        [XmlElement("localizedNameFileRef")]
        public Href<BlankFile> LocalizedNameFileRef { get; set; }
        [XmlElement("localizedDescFileRef")]
        public Href<BlankFile> LocalizedDescFileRef { get; set; }

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
        [XmlArray("addonGroups")] 
        public List<Href<BlankFile>> AddonGroups { get; set; }

        [XmlArrayItem("Item")] 
        public List<Href<BlankFile>> ScriptFileRefs { get; set; }

        public string MainFormId { get; set; }

        [XmlArrayItem("Item")] 
        public List<FormItem> Forms { get; set; }

        [XmlElement("visObjects")]
        public Href<BlankFile> VisObjects { get; set; }
        [XmlElement("aliasVisObjects")]
        public Href<BlankFile> AliasVisObjects { get; set; }
        [XmlElement("texts")]
        public Href<BlankFile> Texts { get; set; }

        [XmlArrayItem("Item")]
        [XmlArray("textsGroups")] 
        public List<TextsItem> TextsGroups { get; set; }

        [XmlElement("textures")]
        public Href<BlankFile> Textures { get; set; }

        [XmlArrayItem("Item")]
        [XmlArray("texturesGroups")] 
        public List<TexturesItem> TexturesGroups { get; set; }

        [XmlElement("sounds")]
        public Href<BlankFile> Sounds { get; set; }

        [XmlArrayItem("Item")]
        [XmlArray("soundsGroups")]
        public List<SoundsItem> SoundsGroups { get; set; }

        [XmlElement("decalObjects")]
        public Href<BlankFile> DecalObjects { get; set; }

        public string Name { get; set; }

        [XmlIgnore] 
        public IEnumerable<IUIElement> Children => Forms?.Select(x => x.Form?.File as IUIElement);

        [XmlIgnore] 
        public WidgetPlacementXY Placement { get; set; }

        [XmlIgnore] 
        public bool Visible { get; set; }

        [XmlIgnore] 
        public bool Enabled { get; set; }

        [XmlIgnore] 
        public ImageSource Bitmap => (Forms?[0].Form?.File as IUIElement)?.Bitmap;

        [XmlIgnore]
        public int ChildrenCount => Children.Count() + Children.Sum(child => child.ChildrenCount);
    }
}