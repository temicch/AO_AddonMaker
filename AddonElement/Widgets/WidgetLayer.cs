using System.Windows.Media;
using Application.BL.Files;

namespace Application.BL.Widgets;

public abstract class WidgetLayer : File
{
    public abstract ImageSource Bitmap { get; }
}
