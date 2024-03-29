﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Serialization;
using Application.BL.Widgets.Layer;

namespace Application.BL.Widgets;

public class WidgetLayerAnimatedTexture : WidgetLayer
{
    public WidgetLayerAnimatedTexture()
    {
        Scaling = false;
        DelayMs = 0;
        RepeatForever = true;
        PlayImmediately = true;
    }

    [XmlIgnore] public bool PlayImmediately { get; set; }

    [XmlElement("playImmidiatly")]
    public string _PlayImmediately
    {
        get => PlayImmediately.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                PlayImmediately = result;
        }
    }

    [XmlIgnore] public bool RepeatForever { get; set; }

    [XmlElement("repeatForever")]
    public string _RepeatForever
    {
        get => RepeatForever.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                RepeatForever = result;
        }
    }

    [XmlIgnore] public bool Scaling { get; set; }

    [XmlElement("scaling")]
    public string _Scaling
    {
        get => Scaling.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                Scaling = result;
        }
    }

    [XmlElement("delayMs")] public int DelayMs { get; set; }

    [XmlArray("frames")] public List<Frame> Frames { get; set; }

    public override ImageSource Bitmap => throw new NotImplementedException();
}
