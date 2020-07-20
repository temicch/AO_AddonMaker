using System.Collections.Generic;
using System.Windows.Media;

namespace Addon.Widgets
{
    public interface IUIElement
    {
        string Name { get; }
        WidgetPlacementXY Placement { get; }
        bool Visible { get; }
        bool Enabled { get; }

        ImageSource Bitmap { get; }

        IEnumerable<IUIElement> Children { get; }
        int ChildrenCount { get; }
    }
}