using System.Globalization;

namespace TLDAG.Core.Globalization
{
    public static class CultureExtensions
    {
        public static bool TryGetRegion(this CultureInfo culture, out RegionInfo? region)
        {
            try
            {
                region = new(culture.LCID);
            }
            catch (System.Exception)
            {
                region = null;
            }

            return region != null;
        }

        public static RegionInfo? GetRegion(this CultureInfo culture)
            => culture.TryGetRegion(out RegionInfo? region) ? region : null;

        public static Currency? GetCurrency(this CultureInfo culture)
            => Currency.GetCurrency(culture);
    }

    public static class RegionExtensions
    {
        public static Currency? GetCurrency(this RegionInfo region)
            => Currency.GetCurrency(region);
    }
}
