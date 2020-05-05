using System;
using System.Collections.Generic;

namespace AO_AddonMaker
{
    public interface IUIElement : IDisposable
    {
        string GetName();
        IEnumerable<IUIElement> GetChildren();
    }
}
