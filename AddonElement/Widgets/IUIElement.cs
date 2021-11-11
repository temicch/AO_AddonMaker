using System.Collections.Generic;
using System.Windows.Media;
using Application.BL.Widgets.Placement;

namespace Application.BL.Widgets;

/// <summary>
///     Provides a description of the graphic item on the screen
/// </summary>
public interface IUIElement
{
    /// <summary>
    ///     Element name
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Element placement
    /// </summary>
    WidgetPlacementXY Placement { get; }

    /// <summary>
    ///     Element visibility
    /// </summary>
    bool Visible { get; }

    /// <summary>
    ///     Enabled element or not
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    ///     Element Image
    /// </summary>
    ImageSource Bitmap { get; }

    /// <summary>
    ///     Element children
    /// </summary>
    IEnumerable<IUIElement> Children { get; }

    /// <summary>
    ///     Children count
    /// </summary>
    int ChildrenCount { get; }
}
