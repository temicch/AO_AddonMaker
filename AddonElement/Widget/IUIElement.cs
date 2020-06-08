﻿using System.Collections.Generic;

namespace AddonElement
{
    public interface IUIElement
    {
        string Name { get; set; }
        WidgetPlacementXY Placement { get; set; }
        bool Visible { get; set; }
        bool Enabled { get; set; }

        IEnumerable<AddonFile> GetChildren();

        List<AddonFile> Widgets { get; }
    }
}
