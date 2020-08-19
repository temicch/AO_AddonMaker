using System.Collections.Generic;

namespace Application.Tests
{
    internal static class InitData
    {
        public static Dictionary<string, int> AddonFilesCount = new Dictionary<string, int>
        {
            ["ItemTemporaryV3"] = 240,
            ["NotifyTrink2"] = 23,
            ["StatsView"] = 54,
            ["TargetShipInfo"] = 199,
            ["UnitDetector"] = 55,
            ["UniverseMeter"] = 367
        };

        public static Dictionary<string, int> AddonUiElementsCount = new Dictionary<string, int>
        {
            ["ItemTemporaryV3"] = 29,
            ["NotifyTrink2"] = 7,
            ["StatsView"] = 10,
            ["TargetShipInfo"] = 39,
            ["UnitDetector"] = 15,
            ["UniverseMeter"] = 90
        };
    }
}