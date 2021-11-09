using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TLDAG.Core.Collections;

namespace TLDAG.Core.Globalization
{
    public static class Cultures
    {
        public static IEnumerable<CultureInfo> AllCultures { get => CultureInfo.GetCultures(CultureTypes.AllCultures); }
    }

    public static class Regions
    {
        public static IEnumerable<RegionInfo> AllRegions { get => Cultures.AllCultures.Select(c => c.GetRegion()).NotNull(); }
    }
}