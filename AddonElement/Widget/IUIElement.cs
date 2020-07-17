using System.Collections.Generic;
using System.Windows.Media;

namespace AddonElement.Widgets
{
    public interface IUIElement
    {
        string Name { get; }
        WidgetPlacementXY Placement { get; }
        bool Visible { get; }
        bool Enabled { get; }

        ImageSource Bitmap { get; }

        List<IUIElement> Children { get; }
    }
}