﻿using System.Xml.Serialization;
using Addon.Files.Provider;

namespace Addon.Widgets
{
    public class TextsItem
    {
        [XmlElement("texts")] public Reference<BlankFileProvider> Texts;

        [XmlElement("groupName")] public string GroupName { get; set; }
    }
}