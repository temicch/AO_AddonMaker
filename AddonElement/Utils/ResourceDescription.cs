using System;
using System.ComponentModel;

namespace Application.BL.Utils
{
    [AttributeUsage(AttributeTargets.All)]
    class ResourceDescription: DescriptionAttribute
    {
        public ResourceDescription(string name): base(GetLocalizedString(name))
        {
        }

        protected static string GetLocalizedString(string key)
        {
            return Resources.Resource.ResourceManager.GetString(key);
        }
    }
}
