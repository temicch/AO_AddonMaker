using System.Collections.Generic;

namespace AO_AddonMaker
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
