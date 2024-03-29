﻿using System.Xml.Serialization;

namespace Application.BL.Widgets.Button;

public struct vec2
{
    [XmlAttribute("X")] public int X { get; set; }

    [XmlAttribute("Y")] public int Y { get; set; }

    public vec2(int x, int y)
    {
        X = x;
        Y = y;
    }
}
