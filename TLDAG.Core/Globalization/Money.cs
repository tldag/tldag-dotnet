using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TLDAG.Core.Globalization
{
    [Serializable]
    public class Currency : IEquatable<Currency>, IComparable<Currency>
    {
        public string IsoCode { get; }

        protected Currency(RegionInfo region) { IsoCode = region.ISOCurrencySymbol; }
        internal Currency() { IsoCode = string.Empty; }

        public override int GetHashCode() => IsoCode.GetHashCode();
        public override bool Equals(object? obj) => EqualsTo(obj as Currency);
        public bool Equals(Currency? other) => EqualsTo(other);
        private bool EqualsTo(Currency? other) => IsoCode.Equals(other?.IsoCode);
        public int CompareTo(Currency? other) => IsoCode.CompareTo(other?.IsoCode);
        public override string ToString() => IsoCode;

        public static bool operator ==(Currency a, Currency b) => a.EqualsTo(b);
        public static bool operator !=(Currency a, Currency b) => !a.EqualsTo(b);

        public static bool operator <(Currency a, Currency b) => a.CompareTo(b) < 0;
        public static bool operator <=(Currency a, Currency b) => a.CompareTo(b) <= 0;
        public static bool operator >(Currency a, Currency b) => a.CompareTo(b) > 0;
        public static bool operator >=(Currency a, Currency b) => a.CompareTo(b) >= 0;

        private static readonly Dictionary<string, Currency> currencies = Regions.AllRegions
            .Select(r => new Currency(r)).Distinct().ToDictionary(c => c.IsoCode);

        public static IEnumerable<Currency> Currencies { get => currencies.Values; }

        public static Currency? GetCurrency(CultureInfo culture)
            => GetCurrency(culture.GetRegion());

        public static Currency? GetCurrency(RegionInfo? region)
            => region is null ? null : GetCurrency(region.ISOCurrencySymbol);

        public static Currency? GetCurrency(string isoCode)
            => currencies.TryGetValue(isoCode, out Currency? currency) ? currency : null;
    }

    public class Money
    {
        public decimal Value { get; }
        public Currency Currency { get; }

        public Money(decimal value, Currency currency) { Value = value; Currency = currency; }

        public override int GetHashCode() => Value.GetHashCode() + Currency.GetHashCode();
    }
}
