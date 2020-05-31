using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AO_AddonMaker
{
    public interface IUIElement
    {
        string Name { get; set; }
        IEnumerable<AddonFile> GetChildren();
        List<AddonFile> Widgets { get; }
    }
}
