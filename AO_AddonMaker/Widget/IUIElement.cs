using System.Collections.Generic;

namespace AO_AddonMaker
{
    public interface IUIElement
    {
        string Name { get; set; }
        IEnumerable<AddonFile> GetChildren();
    }
}
