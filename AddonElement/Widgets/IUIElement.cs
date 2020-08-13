using Application.BL.Widgets.Placement;
using System.Collections.Generic;
using System.Windows.Media;

namespace Application.BL.Widgets
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