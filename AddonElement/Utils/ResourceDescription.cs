using System;
using System.ComponentModel;
using Application.BL.Resources;

namespace Application.BL.Utils;

[AttributeUsage(AttributeTargets.All)]
internal class ResourceDescription : DescriptionAttribute
{
    public ResourceDescription(string name) : base(GetLocalizedString(name))
    {
    }

    protected static string GetLocalizedString(string key)
    {
        return Resource.ResourceManager.GetString(key);
    }
}
