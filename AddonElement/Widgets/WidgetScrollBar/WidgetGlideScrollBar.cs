﻿using System.Xml.Serialization;
using Addon.Files.Provider;

namespace Addon.Widgets
{
    public class WidgetGlideScrollBar : WidgetScrollBar
    {
        [XmlElement("slider")] public Reference<XmlFileProvider> Slider { get; set; }
    }
}