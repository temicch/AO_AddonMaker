﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using Application.BL.Files;
using Application.BL.Files.Provider;
using Application.BL.Widgets.Addon;
using Application.BL.Widgets.Placement;

namespace Application.BL.Widgets;

/// <summary>
///     Addon description file
/// </summary>
[Serializable]
public class UIAddon : File, IUIElement
{
    public UIAddon()
    {
        AutoStart = true;
    }

    [XmlElement("localizedNameFileRef")] public Reference<BlankFileProvider> LocalizedNameFileRef { get; set; }

    [XmlElement("localizedDescFileRef")] public Reference<BlankFileProvider> LocalizedDescFileRef { get; set; }

    [XmlIgnore] public bool AutoStart { get; set; }

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
    public List<Reference<BlankFileProvider>> AddonGroups { get; set; }

    [XmlArrayItem("Item")] public List<Reference<BlankFileProvider>> ScriptFileRefs { get; set; }

    public string MainFormId { get; set; }

    [XmlArrayItem("Item")] public List<FormItem> Forms { get; set; }

    [XmlElement("visObjects")] public Reference<BlankFileProvider> VisObjects { get; set; }

    [XmlElement("aliasVisObjects")] public Reference<BlankFileProvider> AliasVisObjects { get; set; }

    [XmlElement("texts")] public Reference<BlankFileProvider> Texts { get; set; }

    [XmlArrayItem("Item")]
    [XmlArray("textsGroups")]
    public List<TextsItem> TextsGroups { get; set; }

    [XmlElement("textures")] public Reference<BlankFileProvider> Textures { get; set; }

    [XmlArrayItem("Item")]
    [XmlArray("texturesGroups")]
    public List<TexturesItem> TexturesGroups { get; set; }

    [XmlElement("sounds")] public Reference<BlankFileProvider> Sounds { get; set; }

    [XmlArrayItem("Item")]
    [XmlArray("soundsGroups")]
    public List<SoundsItem> SoundsGroups { get; set; }

    [XmlElement("decalObjects")] public Reference<BlankFileProvider> DecalObjects { get; set; }

    public string Name { get; set; }

    [XmlIgnore] public IEnumerable<IUIElement> Children => Forms?.Select(x => x.Form?.File as IUIElement);

    [XmlIgnore] public WidgetPlacementXY Placement { get; set; }

    [XmlIgnore] public bool Visible { get; set; }

    [XmlIgnore] public bool Enabled { get; set; }

    [XmlIgnore] public ImageSource Bitmap => Forms.Count > 0 ? (Forms[0].Form?.File as IUIElement)?.Bitmap : null;

    [XmlIgnore] public int ChildrenCount => Children.Count() + Children.Sum(child => child.ChildrenCount);
}
