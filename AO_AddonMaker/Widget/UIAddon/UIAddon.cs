using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace AO_AddonMaker
{
    [Serializable]
    public class UIAddon : IUIElement
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

        public void Dispose()
        {
            foreach (var item in Forms)
                item.Form.UIElement.Dispose();
            foreach (var item in addonGroups)
                item.UIElement.Dispose();
            foreach (var item in ScriptFileRefs)
                item.UIElement.Dispose();
            foreach (var item in textsGroups)
                item.texts.UIElement.Dispose();
            foreach (var item in texturesGroups)
                item.textures.UIElement.Dispose();
            foreach (var item in soundsGroups)
                item.sounds.UIElement.Dispose();
        }

        [XmlIgnore]
        public string Path { get; set; }
        public string GetPath() => Path;

        public UIAddon()
        {
            WidgetManager.SetRootWidget(this);
            AutoStart = true;
        }

        public string GetName() => Name;

        IEnumerable<IUIElement> IUIElement.GetChildren()
        {
            throw new NotImplementedException();
        }
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
